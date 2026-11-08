
using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class PerfilRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public PerfilRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Perfil>> ObterTodosAsync()
        {
            var perfis = new List<Perfil>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT
                    co_perfil,
                    ds_perfil
                FROM tb_perfil;
            ";

            await using var command = new MySqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var perfil = new Perfil
                {
                    Id = reader.GetInt32("co_perfil"),
                    Descricao = reader.GetString("ds_perfil")
                };

                perfis.Add(perfil);
            }

            return perfis;
        }
    }
}
