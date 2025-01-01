using System.Reflection.Metadata.Ecma335;
using Final_Descent.Configurations;
using MySql.Data.MySqlClient;

namespace Final_Descent.Services
{
    public class ProductService
    {
        private readonly DatabaseConfig _dbConfig;

        public ProductService(DatabaseConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public void TestDatabaseConnection()
        {
            var connectionString = $"Server={_dbConfig.Server};Port={_dbConfig.Port};Database={_dbConfig.Database};User={_dbConfig.User};Password={_dbConfig.Password};";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to the database: {ex.Message}");
                }
            }
        }

        public List<dynamic> GetProducts()
        {
            var products = new List<dynamic>();
            var connectionString = $"Server={_dbConfig.Server};Port={_dbConfig.Port};Database={_dbConfig.Database};User={_dbConfig.User};Password={_dbConfig.Password};";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = "SELECT id, title, image_url, price, original_price, currency, top_selling, choice_badge, choice_text, created_at FROM Products;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var createdAt = reader.GetDateTime("created_at");
                                var productId = reader.GetInt32("id");
                                var price = reader.GetDecimal("price");
                                var originalPrice = reader.GetDecimal("original_price");

                                products.Add(new
                                {
                                    Id = reader.GetInt32("id"),
                                    Title = reader.GetString("title"),
                                    ImageUrl = reader.GetString("image_url"),
                                    Price = reader.GetDecimal("price"),
                                    OriginalPrice = reader.GetDecimal("original_price"),
                                    Currency = reader.GetString("currency"),
                                    IsNew = IsNewProduct(createdAt),
                                    SoldCount = GetSoldCount(productId),
                                    TopSelling = reader.GetBoolean("top_selling"),
                                    DiscountText = GetDiscountText(price, originalPrice),
                                    IsMajorDiscount = IsMajorDiscount(price, originalPrice),
                                    ChoiceBadge = reader["choice_badge"] != DBNull.Value ? (byte[])reader["choice_badge"] : null,
                                    ChoiceText = reader["choice_text"] != DBNull.Value ? reader.GetString("choice_text") : null
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving products: {ex.Message}");
                }
            }

            return products;
        }

        public bool AddProduct(string title, string imageUrl, decimal price, decimal originalPrice, string currency)
        {
            var connectionString = $"Server={_dbConfig.Server};Port={_dbConfig.Port};Database={_dbConfig.Database};User={_dbConfig.User};Password={_dbConfig.Password};";
            
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = "INSERT INTO Products (title, image_url, price, original_price, currency) VALUES (@Title, @ImageUrl, @Price, @OriginalPrice, @Currency);";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@ImageUrl", imageUrl);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@OriginalPrice", originalPrice);
                        command.Parameters.AddWithValue("@Currency", currency);
                        
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding product: {ex.Message}");
                    return false;
                }
            }
        }

        public bool UpdateProduct(int id, string title, string imageUrl, decimal price, decimal originalPrice, string currency)
        {
            var connectionString = $"Server={_dbConfig.Server};Port={_dbConfig.Port};Database={_dbConfig.Database};User={_dbConfig.User};Password={_dbConfig.Password};";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = "UPDATE Products SET title = @Title, image_url = @ImageUrl, price = @Price, original_price = @OriginalPrice, currency = @Currency WHERE id = @Id;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@ImageUrl", imageUrl);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@OriginalPrice", originalPrice);
                        command.Parameters.AddWithValue("@Currency", currency);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating product: {ex.Message}");
                    return false;
                }
            }
        }

        public bool DeleteProduct(int id)
        {
            var connectionString = $"Server={_dbConfig.Server};Port={_dbConfig.Port};Database={_dbConfig.Database};User={_dbConfig.User};Password={_dbConfig.Password};";
            
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = "DELETE FROM Products WHERE id = @Id;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting product: {ex.Message}");
                    return false;
                }
            }
        }

        private bool IsNewProduct(DateTime createdAt)
        {
            return (DateTime.UtcNow - createdAt).TotalDays <= 7;
        }

        private bool IsMajorDiscount(decimal price, decimal originalPrice)
        {
            var discountPercentage = (originalPrice - price) / originalPrice * 100;
            return discountPercentage >= 50;
        }

        private string GetDiscountText(decimal price, decimal originalPrice)
        {
            var discountPercentage = (originalPrice - price) / originalPrice * 100;

            if (discountPercentage >= 50)
            {
                return $"{Math.Round(discountPercentage)}% Off";
            }
            else
            {
                var amountSaved = originalPrice - price;
                return $"Save {amountSaved:C}";
            }
        }

        public int GetSoldCount(int productId)
        {
            // Placeholder for the actual implementation
            return 800;
        }

        public dynamic GetProductById(int id)
        {
            var connectionString = $"Server={_dbConfig.Server};Port={_dbConfig.Port};Database={_dbConfig.Database};User={_dbConfig.User};Password={_dbConfig.Password};";
            
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM Products WHERE id = @Id;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new
                                {
                                    Id = reader.GetInt32("id"),
                                    Title = reader.GetString("title"),
                                    ImageUrl = reader.GetString("image_url"),
                                    Price = reader.GetDecimal("price"),
                                    OriginalPrice = reader.GetDecimal("original_price"),
                                    Currency = reader.GetString("currency"),
                                    MajorDiscount = IsMajorDiscount(reader.GetDecimal("price"), reader.GetDecimal("original_price")),
                                    DiscountText = GetDiscountText(reader.GetDecimal("price"), reader.GetDecimal("original_price"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving product: {ex.Message}");
                }
            }
            return null;
        }
    }
}
