using Final_Descent.Configurations;
using Final_Descent.Data;
using Final_Descent.Services;
using Microsoft.EntityFrameworkCore;

namespace Final_Descent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add environment variables to configuration
            builder.Configuration.AddEnvironmentVariables();

            // Register the Database Configuration Class
            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new DatabaseConfig(configuration);
            });

            // Register the Product Service
            builder.Services.AddSingleton<ProductService>();

            // Register the Email Service
            builder.Services.AddSingleton<EmailService>();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");

            // Register the Registration Service
            builder.Services.AddSingleton(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                var emailService = sp.GetRequiredService<EmailService>();

                return new RegistrationService(connectionString, emailService);
            });

            // Enable Session Management
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "FinalDescent.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromHours(48);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            builder.Services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = 7287;
            });

            // Register the Chapa Service
            builder.Services.AddHttpClient<ChapaService>();
            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var chapaPublicKey = configuration["Chapa:PublicKey"] 
                    ?? throw new InvalidOperationException("Chapa Public Key not found in configuration.");

                return new ChapaService(chapaPublicKey);
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Final Descent API v1");
                });

                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseMiddleware<Final_Descent.Middleware.SessionValidationMiddleware>();

            app.Use(async (context, next) =>
            {
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
                await next();
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
