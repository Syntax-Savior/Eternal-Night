using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Descent.Models
{
    public class UnverifiedUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string OTP { get; set; }
        public DateTime? OTPExpiration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string ConfirmPassword { get; set; } // Temporary property for validation
    }
}
