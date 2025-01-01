using MySql.Data.MySqlClient;

namespace Final_Descent.Configurations
{
    public class DatabaseConfig
    {
        public string Server { get; private set; }
        public int Port { get; private set; }
        public string Database { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }

        public DatabaseConfig(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var builder = new MySqlConnectionStringBuilder(connectionString);
            Server = builder.Server;
            Port = (int)builder.Port;
            Database = builder.Database;
            User = builder.UserID;
            Password = builder.Password;
        }
    }
}
