using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class CondicaoJogoRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CondicaoJogoRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<CondicaoJogo>> ObterTodosAsync()
        {
            var condicoes = new List<CondicaoJogo>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT
                    co_condicao_jogo,
                    ds_condicao_jogo
                FROM tb_condicao_jogo;
            ";

            await using var command = new MySqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var condicao = new CondicaoJogo
                {
                    Id = reader.GetInt32("co_condicao_jogo"),
                    Descricao = reader.GetString("ds_condicao_jogo")
                };

                condicoes.Add(condicao);
            }

            return condicoes;
        }
    }
}