using Final_Descent.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.VisualBasic;

namespace Final_Descent.Services
{
    public class RegistrationService
    {
        private readonly string _connectionString;
        private readonly IDataProtector _dataProtector;
        private readonly EmailService _emailService;

        public RegistrationService(string connectionString, EmailService emailService)
        {
            _connectionString = connectionString;

            var dataProtectionProvider = DataProtectionProvider.Create("FinalDescent");
            _dataProtector = dataProtectionProvider.CreateProtector("SensitiveDataProtector");
            _emailService = emailService;
        }

        private string EncryptData(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException(nameof(data), "Data to encrypt cannot be null or empty");

            return _dataProtector.Protect(data);
        }

        private string DecryptData(string encryptedData)
        {
            if (string.IsNullOrEmpty(encryptedData))
                throw new ArgumentNullException(nameof(encryptedData), "Data to decrypt cannot be null or empty");

            return _dataProtector.Unprotect(encryptedData);
        }

        public bool SaveUnverifiedUser(UnverifiedUser user)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            
            const string query = "INSERT INTO UnverifiedUsers (FullName, Email, PasswordHash) VALUES (@FullName, @Email, @PasswordHash)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@FullName", user.FullName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateOTP(string email, string otp, DateTime expiration)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            const string selectQuery = "SELECT LastOtpRequest FROM UnverifiedUsers WHERE Email = @Email";
            using var selectCommand = new MySqlCommand(selectQuery, connection);
            selectCommand.Parameters.AddWithValue("@Email", email);

            var lastRequest = selectCommand.ExecuteScalar() as DateTime?;

            if (lastRequest != null && lastRequest.Value.AddMinutes(2) > DateTime.UtcNow)
            {
                return false;
            }

            const string updateQuery = @"UPDATE UnverifiedUsers 
                                         SET OTP = @OTP, OTPExpiration = @Expiration, LastOtpRequest = @Now 
                                         WHERE Email = @Email";

            using var updateCommand = new MySqlCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@OTP", otp);
            updateCommand.Parameters.AddWithValue("@Expiration", expiration);
            updateCommand.Parameters.AddWithValue("@Now", DateTime.UtcNow);
            updateCommand.Parameters.AddWithValue("@Email", email);

            if (updateCommand.ExecuteNonQuery() > 0)
            {
                var emailBody = $"<p>Your OTP is <strong>{otp}</strong>. It will expire in 5 minutes.<br><hr>Please do not share your code with anyone.</p>";
                _emailService.SendEmailAsync(email, "Your OTP Code", emailBody).Wait();

                return true;
            }

            return false;
        }

        public UnverifiedUser GetUnverifiedUserByEmail(string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            const string query = "SELECT * FROM UnverifiedUsers WHERE Email = @Email";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UnverifiedUser
                {
                    Id = reader.GetInt32("Id"),
                    FullName = reader.GetString("FullName"),
                    Email = reader.GetString("Email"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    OTP = reader["OTP"] != DBNull.Value ? reader.GetString("OTP") : null,
                    OTPExpiration = reader["OTPExpiration"] != DBNull.Value ? reader.GetDateTime("OTPExpiration") : null,
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };
            }

            return null;
        }

        public bool AddPhoneNumber(string email, string phoneNumber)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                var encryptedPhoneNumber = EncryptData(phoneNumber);

                const string insertQuery = @"INSERT INTO Users (FullName, Email, PasswordHash, PhoneNumber)
                                             SELECT FullName, Email, PasswordHash, @PhoneNumber
                                             FROM UnverifiedUsers WHERE Email = @Email";

                using var insertCommand = new MySqlCommand(insertQuery, connection, transaction);
                insertCommand.Parameters.AddWithValue("@Email", email);
                insertCommand.Parameters.AddWithValue("@PhoneNumber", encryptedPhoneNumber);

                if (insertCommand.ExecuteNonQuery() == 0)
                {
                    transaction.Rollback();
                    return false;
                }

                const string deleteQuery = "DELETE FROM UnverifiedUsers WHERE Email = @Email";
                using var deleteCommand = new MySqlCommand(deleteQuery, connection, transaction);
                deleteCommand.Parameters.AddWithValue("@Email", email);

                if (deleteCommand.ExecuteNonQuery() == 0)
                {
                    transaction.Rollback();
                    return false;
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AddPhoneNumber transaction failed: {ex.Message}");
                transaction.Rollback();
                throw;
            }
        }

        public VerifiedUser GetVerifiedUserByEmail(string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            const string query = "SELECT Email, PasswordHash FROM Users WHERE Email = @Email";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new VerifiedUser
                {
                    Email = reader["Email"].ToString(),
                    PasswordHash = reader["PasswordHash"].ToString()
                };
            }

            return null;
        }
    }
}
