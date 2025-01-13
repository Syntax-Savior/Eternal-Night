using Final_Descent.Models;
using Final_Descent.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Final_Descent.Controllers
{
    [Route("RazorViewsRegistrationController")]
    public class RazorViewsRegistrationController : Controller
    {
        private readonly RegistrationService _registrationService;

        public RazorViewsRegistrationController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet("signup")]
        public IActionResult Signup()
        {
            return View("~/Views/Pages/Signup.cshtml");
        }

        [HttpPost("signup")]
        public IActionResult Signup(UnverifiedUser user)
        {
            if (_registrationService.GetUnverifiedUserByEmail(user.Email) != null)
            {
                TempData["ErrorMessage"] = "Email is already in Use.";
                return View("~/Views/Pages/Signup.cshtml");
            }

            if (user.PasswordHash != user.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match.";
                return View("~/Views/Pages/Signup.cshtml");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            if (_registrationService.SaveUnverifiedUser(user))
            {
                HttpContext.Session.SetString("UserEmail", user.Email);

                var otp = new Random().Next(100000, 999999).ToString();
                var expiration = DateTime.UtcNow.AddMinutes(5);

                if (!_registrationService.UpdateOTP(user.Email, otp, expiration))
                {
                    TempData["ErrorMessage"] = "You can only request an OTP every 2 minutes. Please try again later.";
                    return View("~/Views/Pages/Signup.cshtml");
                }

                Console.WriteLine($"Generated OTP for {user.Email}: {otp}");
                return RedirectToAction("OtpRequestPage");
            }

            TempData["ErrorMessage"] = "An error occurred while registering the user.";
            return View("~/Views/Pages/Signup.cshtml");
        }

        [HttpGet("otp-request-page")]
        public IActionResult OtpRequestPage()
        {
            return View("~/Views/Pages/OtpRequestPage.cshtml");
        }
        
        [HttpPost("otp-request-page")]
        public IActionResult OtpRequestPage(string otp)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            Console.WriteLine($"[DEBUG] Session UserEmail in OtpRequestPage: {email}");

            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Session expired. Please register again.";
                return RedirectToAction("Signup");
            }

            var user = _registrationService.GetUnverifiedUserByEmail(email);
            if (user == null || user.OTP != otp || user.OTPExpiration < DateTime.UtcNow)
            {
                TempData["ErrorMessage"] = "Invalid or expired OTP. Please try again.";
                return View("~/Views/Pages/OtpRequestPage.cshtml");
            }

            return RedirectToAction("AddPhoneNumberPage");
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOTP(string otp)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                ViewData["ErrorMessage"] = "Session expired. Please restart the registration process.";
                return View("~/Views/Pages/OtpRequestPage.cshtml");
            }

            var user = _registrationService.GetUnverifiedUserByEmail(email);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "User not found.";
                return View("~/Views/Pages/OtpRequestPage.cshtml");
            }

            if (user.OTP != otp || user.OTPExpiration < DateTime.UtcNow)
            {
                ViewData["ErrorMessage"] = "Invalid or expired OTP.";
                return View("~/Views/Pages/OtpRequestPage.cshtml");
            }

            return RedirectToAction("AddPhoneNumberPage");
        }

        [HttpGet("add-phone-number")]
        public IActionResult AddPhoneNumberPage()
        {
            return View("~/Views/Pages/AddPhoneNumberPage.cshtml");
        }


        [HttpPost("add-phone-number")]
        public IActionResult AddPhoneNumber(string phoneNumber)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                ViewData["ErrorMessage"] = "Session expired. Please restart the registration process.";
                return RedirectToAction("Signup");
            }

            if (_registrationService.AddPhoneNumber(email, phoneNumber))
            {
                HttpContext.Session.Remove("UserEmail");

                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("UserEmail", email);

                var protector = HttpContext.RequestServices
                    .GetDataProtector("FinalDescent.Cookie.Protector");

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(7),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true
                };

                var encryptedIsLoggedIn = protector.Protect("true");
                var encryptedUserEmail = protector.Protect(email);

                Response.Cookies.Append("IsLoggedIn", encryptedIsLoggedIn, cookieOptions);
                Response.Cookies.Append("UserEmail", encryptedUserEmail, cookieOptions);

                return RedirectToAction("Index", "Product");
            }

            ViewData["ErrorMessage"] = "An error occurred while adding your phone number. Please try again.";
            return View("~/Views/Pages/AddPhoneNumberPage.cshtml");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("~/Views/Pages/Login.cshtml");
        }

        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Email and password are required.";
                return View("~/Views/Pages/Login.cshtml");
            }

            var user = _registrationService.GetVerifiedUserByEmail(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return View("~/Views/Pages/Login.cshtml");
            }

            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("IsLoggedIn", "true");

            var protector = HttpContext.RequestServices
                .GetDataProtector("FinalDescent.Cookie.Protector");

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true,
                Secure = true
            };

            var encryptedIsLoggedIn = protector.Protect("true");
            var encryptedUserEmail = protector.Protect(email);

            Response.Cookies.Append("IsLoggedIn", encryptedIsLoggedIn, cookieOptions);
            Response.Cookies.Append("UserEmail", encryptedUserEmail, cookieOptions);

            return RedirectToAction("Index", "Product");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            Response.Cookies.Delete("IsLoggedIn");
            Response.Cookies.Delete("UserEmail");

            return RedirectToAction("Index", "Product");
        }
    }
}
