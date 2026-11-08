using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class PlataformaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public PlataformaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Plataforma>> ObterTodosAsync()
        {
            var plataformas = new List<Plataforma>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT
                    co_seq_plataforma,
                    no_plataforma,
                    no_fabricante,
                    nu_ano_lancamento
                FROM tb_plataforma;
            ";

            await using var command = new MySqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var plataforma = new Plataforma
                {
                    Id = reader.GetInt32("co_seq_plataforma"),
                    Nome = reader.GetString("no_plataforma"),
                    Fabricante = reader.GetString("no_fabricante"),
                    AnoLancamento = reader.GetInt32("nu_ano_lancamento")
                };

                plataformas.Add(plataforma);
            }

            return plataformas;
        }
    }
}