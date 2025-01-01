using Final_Descent.Models;
using Final_Descent.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Final_Descent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly RegistrationService _registrationService;

        public RegistrationController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UnverifiedUser user)
        {
            if (_registrationService.GetUnverifiedUserByEmail(user.Email) != null)
            {
                return BadRequest("Email is already in use.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            if (_registrationService.SaveUnverifiedUser(user))
            {
                return Ok("User registered successfully. Please verify your email.");
            }

            return StatusCode(500, "An error occurred while registering the user.");
        }

        [HttpPost("send-otp")]
        public IActionResult SendOTP([FromBody] string email)
        {
            var user = _registrationService.GetUnverifiedUserByEmail(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var otp = new Random().Next(100000, 999999).ToString();
            var expiration = DateTime.UtcNow.AddMinutes(5);

            if (_registrationService.UpdateOTP(email, otp, expiration))
            {
                // Simulate sending email (replace with real email logic)
                Console.WriteLine($"OTP for {email}: {otp}");
                return Ok("OTP sent successfully.");
            }

            return StatusCode(500, "An error occurred while sending the OTP.");
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOTP([FromBody] VerifyOtpRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.OTP))
            {
                return BadRequest("Email and OTP are required.");
            }

            var user = _registrationService.GetUnverifiedUserByEmail(request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.OTP != request.OTP || user.OTPExpiration < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired OTP.");
            }

            return Ok("OTP verified. Proceed to finalize registration.");
        }

        [HttpPost("finalize-registration")]
        public IActionResult FinalizeRegistration([FromBody] FinalizeRegistrationRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest("Email and phone number are required.");
            }

            if (_registrationService.FinalizeRegistration(request.Email, request.PhoneNumber))
            {
                return Ok("Registration completed successfully.");
            }

            return StatusCode(500, "An error occurred while finalizing registration.");
        }
    }
}
