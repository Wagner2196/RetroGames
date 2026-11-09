
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class CondicaoJogoRepository
        : RepositoryBase, ICondicaoJogoRepository
    {
        public CondicaoJogoRepository(DbConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<List<CondicaoJogo>> ObterTodosAsync()
        {
            var condicoes = new List<CondicaoJogo>();

            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                SELECT
                    co_condicao_jogo,
                    ds_condicao_jogo
                FROM tb_condicao_jogo;
            ";

            await using var command = CriarComando(connection, sql);
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
