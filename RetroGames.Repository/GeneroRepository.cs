using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class GeneroRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public GeneroRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Genero>> ObterTodosAsync()
        {
            var generos = new List<Genero>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT
                    co_seq_genero,
                    no_genero
                FROM tb_genero;
            ";

            await using var command = new MySqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var genero = new Genero
                {
                    Id = reader.GetInt32("co_seq_genero"),
                    Nome = reader.GetString("no_genero")
                };

                generos.Add(genero);
            }

            return generos;
        }
    }
}