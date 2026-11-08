using MySqlConnector;

namespace RetroGames.Repository
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory()
        {
            _connectionString =
                "Server=localhost;Port=3306;Database=RetroGames;User ID=root;Password=Wagner2100;";
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}