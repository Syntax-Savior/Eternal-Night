using Final_Descent.Models;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using Microsoft.VisualBasic;

namespace Final_Descent.Services
{
    public class RegistrationService
    {
        private readonly string _connectionString;

        public RegistrationService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool SaveUnverifiedUser(UnverifiedUser user)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO UnverifiedUsers (FullName, Email, PasswordHash) VALUES (@FullName, @Email, @PasswordHash)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", user.FullName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateOTP(string email, string otp, DateTime expiration)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE UnverifiedUsers SET OTP = @OTP, OTPExpiration = @Expiration WHERE Email = @Email";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OTP", otp);
                    command.Parameters.AddWithValue("@Expiration", expiration);
                    command.Parameters.AddWithValue("@Email", email);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public UnverifiedUser GetUnverifiedUserByEmail(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM UnverifiedUsers WHERE Email = @Email";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (var reader = command.ExecuteReader())
                    {
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
                }
            }
        }

        public bool FinalizeRegistration(string email, string phoneNumber)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"INSERT INTO Users (FullName, Email, PasswordHash, PhoneNumber)
                              SELECT FullName, Email, PasswordHash, @PhoneNumber
                              FROM UnverifiedUsers WHERE Email = @Email;

                              DELETE FROM UnverifiedUsers WHERE Email = @Email;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
