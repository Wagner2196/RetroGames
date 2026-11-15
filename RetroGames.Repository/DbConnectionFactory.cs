
using MySqlConnector;

namespace RetroGames.Repository
{
    // Responsável por criar conexões com o banco de dados.
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        // Recebe a configuração centralizada dos repositórios.
        public DbConnectionFactory(RepositoryConfig config)
        {
            _connectionString =
                        "Server=localhost;Port=3306;Database=RetroGames;User ID=root;Password=Wagner2100;";

        }

        // Cria uma nova conexão utilizando a configuração recebida.
        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
