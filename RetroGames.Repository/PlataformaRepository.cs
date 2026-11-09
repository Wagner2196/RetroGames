
using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class PlataformaRepository
        : RepositoryBase, IPlataformaRepository
    {
        public PlataformaRepository(DbConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<List<Plataforma>> ListarAsync()
        {
            var plataformas = new List<Plataforma>();

            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                SELECT
                    co_seq_plataforma,
                    no_plataforma,
                    no_fabricante,
                    nu_ano_lancamento
                FROM tb_plataforma
                ORDER BY no_plataforma;
            ";

            await using var command = CriarComando(connection, sql);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                plataformas.Add(new Plataforma
                {
                    Id = reader.GetInt32("co_seq_plataforma"),
                    Nome = reader.GetString("no_plataforma"),
                    Fabricante = reader.GetString("no_fabricante"),
                    AnoLancamento = reader.GetInt32("nu_ano_lancamento")
                });
            }

            return plataformas;
        }

        public async Task<Plataforma?> BuscarPorIdAsync(int id)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                SELECT
                    co_seq_plataforma,
                    no_plataforma,
                    no_fabricante,
                    nu_ano_lancamento
                FROM tb_plataforma
                WHERE co_seq_plataforma = @id;
            ";

            await using var command = CriarComando(connection, sql);
            AdicionarParametro(command, "@id", id);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new Plataforma
            {
                Id = reader.GetInt32("co_seq_plataforma"),
                Nome = reader.GetString("no_plataforma"),
                Fabricante = reader.GetString("no_fabricante"),
                AnoLancamento = reader.GetInt32("nu_ano_lancamento")
            };
        }

        public async Task<bool> ExisteComNomeAsync(
            string nome,
            int? idIgnorar = null)
        {
            await using var connection = await AbrirConexaoAsync();

            var sql = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM tb_plataforma
                    WHERE no_plataforma = @nome
            ";

            if (idIgnorar.HasValue)
            {
                sql += " AND co_seq_plataforma <> @idIgnorar";
            }

            sql += ");";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@nome", nome);

            if (idIgnorar.HasValue)
            {
                AdicionarParametro(command, "@idIgnorar", idIgnorar.Value);
            }

            var resultado = await command.ExecuteScalarAsync();

            return Convert.ToBoolean(resultado);
        }

        public async Task<bool> PossuiJogosAsync(int id)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM tb_jogo
                    WHERE co_plataforma = @id
                );
            ";

            await using var command = CriarComando(connection, sql);
            AdicionarParametro(command, "@id", id);

            var resultado = await command.ExecuteScalarAsync();

            return Convert.ToBoolean(resultado);
        }

        public async Task<int> InserirAsync(Plataforma plataforma)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                INSERT INTO tb_plataforma (
                    no_plataforma,
                    no_fabricante,
                    nu_ano_lancamento
                )
                VALUES (
                    @nome,
                    @fabricante,
                    @anoLancamento
                );
            ";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@nome", plataforma.Nome);
            AdicionarParametro(command, "@fabricante", plataforma.Fabricante);
            AdicionarParametro(command, "@anoLancamento", plataforma.AnoLancamento);

            await ExecutarGravacaoAsync(command);

            return Convert.ToInt32(command.LastInsertedId);
        }

        public async Task AlterarAsync(Plataforma plataforma)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                UPDATE tb_plataforma
                SET
                    no_plataforma = @nome,
                    no_fabricante = @fabricante,
                    nu_ano_lancamento = @anoLancamento
                WHERE co_seq_plataforma = @id;
            ";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@id", plataforma.Id);
            AdicionarParametro(command, "@nome", plataforma.Nome);
            AdicionarParametro(command, "@fabricante", plataforma.Fabricante);
            AdicionarParametro(command, "@anoLancamento", plataforma.AnoLancamento);

            await ExecutarGravacaoAsync(command);
        }

        public async Task ExcluirAsync(int id)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                DELETE FROM tb_plataforma
                WHERE co_seq_plataforma = @id;
            ";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@id", id);

            await ExecutarGravacaoAsync(command);
        }
    }
}
