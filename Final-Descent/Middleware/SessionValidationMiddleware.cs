using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Final_Descent.Controllers;
using Microsoft.AspNetCore.DataProtection;

namespace Final_Descent.Middleware
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var isLoggedIn = context.Session.GetString("IsLoggedIn");

                if (string.IsNullOrEmpty(isLoggedIn))
                {
                    var protector = context.RequestServices.GetDataProtector("FinalDescent.Cookie.Protector");

                    var encryptedIsLoggedIn = context.Request.Cookies["IsLoggedIn"];
                    var encryptedUserEmail = context.Request.Cookies["UserEmail"];

                    if (!string.IsNullOrEmpty(encryptedIsLoggedIn) && !string.IsNullOrEmpty(encryptedUserEmail))
                    {
                        try
                        {
                            var decryptedIsLoggedIn = protector.Unprotect(encryptedIsLoggedIn);
                            var decryptedUserEmail = protector.Unprotect(encryptedUserEmail);

                            if (decryptedIsLoggedIn == "true")
                            {
                                context.Session.SetString("IsLoggedIn", "true");
                                context.Session.SetString("UserEmail", decryptedUserEmail);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error decrypting cookies: {ex.Message}");
                            context.Response.Cookies.Delete("IsLoggedIn");
                            context.Response.Cookies.Delete("UserEmail");
                        }
                    }
                }

                if (IsWhitelistedPath(context.Request.Path))
                {
                    await _next(context);
                    return;
                }

                if (context.Session.GetString("IsLoggedIn") != "true")
                {
                    Console.WriteLine($"[DEBUG] Session invalid. Redirecting to homepage. Path: {context.Request.Path}");
                    context.Session.Clear();
                    context.Response.Redirect("/");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Middleware exception: {ex.Message}");
                context.Response.Redirect("/Error");
                return;
            }

            await _next(context);
        }

        private static bool IsWhitelistedPath(PathString path)
        {
            return path.StartsWithSegments("/RazorViewsRegistrationController/signup") ||
                   path.StartsWithSegments("/RazorViewsRegistrationController/login") ||
                   path.StartsWithSegments("/RazorViewsRegistrationController/otp-request-page") ||
                   path.StartsWithSegments("/RazorViewsRegistrationController/add-phone-number") ||
                   path.StartsWithSegments("/RazorViewsRegistrationController/verify-otp") ||
                   path.StartsWithSegments("/") ||
                   path.StartsWithSegments("/js") ||
                   path.StartsWithSegments("/css") ||
                   path.StartsWithSegments("/Assets") ||
                   path.StartsWithSegments("/lib");
        }
    }
}
