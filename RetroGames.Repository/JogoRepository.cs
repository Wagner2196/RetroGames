using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class JogoRepository
        : RepositoryBase, IJogoRepository
    {
        public JogoRepository(DbConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<PaginaResultado<Jogo>> ListarAsync(
            string? nome = null,
            int? idPlataforma = null,
            List<int>? ids = null,
            bool incluirInativos = false,
            int pagina = 1,
            int tamanhoPagina = 10)
        {
            var resultado = new PaginaResultado<Jogo>();

            if (pagina < 1)
                throw new ArgumentOutOfRangeException(nameof(pagina));

            if (tamanhoPagina < 1)
                throw new ArgumentOutOfRangeException(nameof(tamanhoPagina));

            var filtros = new List<string>();
            var parametros = new Dictionary<string, object?>();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                filtros.Add("no_jogo LIKE @nome");
                parametros.Add("@nome", $"%{nome}%");
            }

            if (idPlataforma.HasValue)
            {
                filtros.Add("co_plataforma = @idPlataforma");
                parametros.Add("@idPlataforma", idPlataforma.Value);
            }

            if (ids != null && ids.Count > 0)
            {
                var nomesParametros = new List<string>();

                for (int i = 0; i < ids.Count; i++)
                {
                    var nomeParametro = $"@id{i}";

                    nomesParametros.Add(nomeParametro);
                    parametros.Add(nomeParametro, ids[i]);
                }

                filtros.Add(
                    $"co_seq_jogo IN ({string.Join(", ", nomesParametros)})");
            }

            if (!incluirInativos)
            {
                filtros.Add("st_ativo = 1");
            }

            var where = filtros.Count > 0
                ? $"WHERE {string.Join(" AND ", filtros)}"
                : string.Empty;

            var offset = (pagina - 1) * tamanhoPagina;

            await using var connection = await AbrirConexaoAsync();

            var sqlLista = $@"
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
                FROM tb_jogo
                {where}
                ORDER BY no_jogo
                LIMIT @limite OFFSET @offset;
            ";

            await using (var command = CriarComando(connection, sqlLista))
            {
                foreach (var parametro in parametros)
                {
                    AdicionarParametro(
                        command,
                        parametro.Key,
                        parametro.Value);
                }

                AdicionarParametro(command, "@limite", tamanhoPagina);
                AdicionarParametro(command, "@offset", offset);

                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    resultado.Itens.Add(MapearJogo(reader));
                }
            }

            var sqlTotal = $@"
                SELECT COUNT(*)
                FROM tb_jogo
                {where};
            ";

            await using (var command = CriarComando(connection, sqlTotal))
            {
                foreach (var parametro in parametros)
                {
                    AdicionarParametro(
                        command,
                        parametro.Key,
                        parametro.Value);
                }

                var total = await command.ExecuteScalarAsync();

                resultado.Total = Convert.ToInt32(total);
            }

            return resultado;
        }

        public async Task<Jogo?> BuscarPorIdAsync(int id)
        {
            await using var connection = await AbrirConexaoAsync();

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
                FROM tb_jogo
                WHERE co_seq_jogo = @id;
            ";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@id", id);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapearJogo(reader);
        }

        public async Task<int> InserirAsync(Jogo jogo)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                INSERT INTO tb_jogo (
                    no_jogo,
                    ds_jogo,
                    nu_ano_lancamento,
                    vl_preco,
                    qt_estoque,
                    st_ativo,
                    dt_cadastro,
                    co_plataforma,
                    co_condicao_jogo
                )
                VALUES (
                    @nome,
                    @descricao,
                    @anoLancamento,
                    @preco,
                    @estoque,
                    @ativo,
                    @dataCadastro,
                    @idPlataforma,
                    @idCondicaoJogo
                );
            ";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@nome", jogo.Nome);
            AdicionarParametro(command, "@descricao", jogo.Descricao);
            AdicionarParametro(command, "@anoLancamento", jogo.AnoLancamento);
            AdicionarParametro(command, "@preco", jogo.Preco);
            AdicionarParametro(command, "@estoque", jogo.Estoque);
            AdicionarParametro(command, "@ativo", jogo.Ativo);
            AdicionarParametro(command, "@dataCadastro", jogo.DataCadastro);
            AdicionarParametro(command, "@idPlataforma", jogo.IdPlataforma);
            AdicionarParametro(command, "@idCondicaoJogo", jogo.IdCondicaoJogo);

            await ExecutarGravacaoAsync(command);

            return Convert.ToInt32(command.LastInsertedId);
        }

        public async Task AlterarAsync(Jogo jogo)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                UPDATE tb_jogo
                SET
                    no_jogo = @nome,
                    ds_jogo = @descricao,
                    nu_ano_lancamento = @anoLancamento,
                    vl_preco = @preco,
                    qt_estoque = @estoque,
                    st_ativo = @ativo,
                    co_plataforma = @idPlataforma,
                    co_condicao_jogo = @idCondicaoJogo
                WHERE co_seq_jogo = @id;
            ";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@id", jogo.Id);
            AdicionarParametro(command, "@nome", jogo.Nome);
            AdicionarParametro(command, "@descricao", jogo.Descricao);
            AdicionarParametro(command, "@anoLancamento", jogo.AnoLancamento);
            AdicionarParametro(command, "@preco", jogo.Preco);
            AdicionarParametro(command, "@estoque", jogo.Estoque);
            AdicionarParametro(command, "@ativo", jogo.Ativo);
            AdicionarParametro(command, "@idPlataforma", jogo.IdPlataforma);
            AdicionarParametro(command, "@idCondicaoJogo", jogo.IdCondicaoJogo);

            await ExecutarGravacaoAsync(command);
        }

        public async Task DesativarAsync(int id)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                UPDATE tb_jogo
                SET st_ativo = 0
                WHERE co_seq_jogo = @id;
            ";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@id", id);

            await ExecutarGravacaoAsync(command);
        }

        private static Jogo MapearJogo(MySqlConnector.MySqlDataReader reader)
        {
            return new Jogo
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
        }
    }
}