using Final_Descent.Models;
using Final_Descent.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Final_Descent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIsRegistrationController : ControllerBase
    {
        private readonly RegistrationService _registrationService;

        public APIsRegistrationController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UnverifiedUser user)
        {
            try
            {
                if (_registrationService.GetUnverifiedUserByEmail(user.Email) != null)
                {
                    return BadRequest(new { Message = "Email is already in Use." });
                }

                if (user.PasswordHash != user.ConfirmPassword)
                {
                    return BadRequest(new { Message = "Passwords do not match." });
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                if (_registrationService.SaveUnverifiedUser(user))
                {
                    var otp = new Random().Next(100000, 999999).ToString();
                    var expiration = DateTime.UtcNow.AddMinutes(5);

                    if (!_registrationService.UpdateOTP(user.Email, otp, expiration))
                    {
                        return BadRequest(new { Message = "You can only request an OTP every 2 minutes. Please try again later." });
                    }

                    Console.WriteLine($"Generated OTP for {user.Email}: {otp}");

                    return Ok(new { Message = "User registered successfully. Please verify your email." });
                }

                return StatusCode(500, "An error occurred while registering the user.");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error in Register method: {ex.Message}");
                return StatusCode(500, "Internal Server Error. Please try again.");
            }
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOTP([FromBody] string otp)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new { Message = "Session expired. Please restart the registration process." });
            }

            var user = _registrationService.GetUnverifiedUserByEmail(email);
            if (user == null || user.OTP != otp || user.OTPExpiration < DateTime.UtcNow)
            {
                return BadRequest(new { Message = "Invalid or expired OTP." });
            }

            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("IsLoggedIn", "true");

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true
            };
            Response.Cookies.Append("IsLoggedIn", "true", cookieOptions);
            Response.Cookies.Append("UserEmail", email, cookieOptions);

            return Ok(new { Message = "OTP verified successfully. Proceed to add phone number." });
        }

        [HttpPost("add-phone-number")]
        public IActionResult AddPhoneNumber([FromBody] string phoneNumber)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new { Message = "Session expired. Please restart the registration process." });
            }

            if (_registrationService.AddPhoneNumber(email, phoneNumber))
            {
                HttpContext.Session.Remove("UserEmail");
                HttpContext.Session.SetString("IsLoggedIn", "true");

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(7),
                    HttpOnly = true,
                    IsEssential = true
                };
                Response.Cookies.Append("IsLoggedIn", "true", cookieOptions);
                Response.Cookies.Append("UserEmail", email, cookieOptions);

                return Ok(new { Message = "Registration completed successfully." });
            }

            return StatusCode(500, "An error occurred while finalizing registration.");
        }
    }
}
