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

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");

            // Register the Registration Service
            builder.Services.AddSingleton(new RegistrationService(connectionString));

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
                app.UseSwaggerUI();

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
            
            app.UseRouting();

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
