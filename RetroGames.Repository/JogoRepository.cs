
using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class JogoRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public JogoRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Jogo>> ObterTodosAsync()
        {
            var jogos = new List<Jogo>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT
                    co_seq_jogo,
                    no_jogo,
                    ds_jogo,
                    nu_ano_lancamento,
                    vl_preco,
                    qt_estoque,
                    st_ativo,
                    dt_cadastro,
                    co_plataforma,
                    co_condicao_jogo
                FROM tb_jogo;
            ";

            await using var command = new MySqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var jogo = new Jogo
                {
                    Id = reader.GetInt32("co_seq_jogo"),
                    Nome = reader.GetString("no_jogo"),
                    Descricao = reader.IsDBNull(reader.GetOrdinal("ds_jogo"))
                        ? null
                        : reader.GetString("ds_jogo"),
                    AnoLancamento = reader.GetInt32("nu_ano_lancamento"),
                    Preco = reader.GetDecimal("vl_preco"),
                    Estoque = reader.GetInt32("qt_estoque"),
                    Ativo = reader.GetBoolean("st_ativo"),
                    DataCadastro = reader.GetDateTime("dt_cadastro"),
                    IdPlataforma = reader.GetInt32("co_plataforma"),
                    IdCondicaoJogo = reader.GetInt32("co_condicao_jogo")
                };

                jogos.Add(jogo);
            }

            return jogos;
        }
    }
}
