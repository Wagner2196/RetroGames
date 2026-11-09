
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    // Implementa as operações de acesso à tabela tb_jogo_genero.
    // Este Repository consulta somente essa tabela, sem JOIN.
    public class JogoGeneroRepository
        : RepositoryBase, IJogoGeneroRepository
    {
        public JogoGeneroRepository(DbConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        // Lista todos os vínculos entre jogos e gêneros.
        public async Task<List<JogoGenero>> ListarTodosAsync()
        {
            var vinculos = new List<JogoGenero>();

            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                SELECT
                    co_jogo,
                    co_genero
                FROM tb_jogo_genero
                ORDER BY co_jogo, co_genero;
            ";

            await using var command = CriarComando(connection, sql);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                vinculos.Add(new JogoGenero
                {
                    IdJogo = reader.GetInt32("co_jogo"),
                    IdGenero = reader.GetInt32("co_genero")
                });
            }

            return vinculos;
        }

        // Lista os vínculos de vários jogos usando uma única consulta.
        public async Task<List<JogoGenero>> ListarPorJogosAsync(
            List<int> idsJogos)
        {
            var vinculos = new List<JogoGenero>();

            // Se a lista estiver vazia, não há vínculos a consultar.
            if (idsJogos == null || idsJogos.Count == 0)
                return vinculos;

            await using var connection = await AbrirConexaoAsync();

            // Cria um parâmetro individual para cada ID.
            // Os valores são parametrizados para evitar SQL injection.
            var parametrosIds = new List<string>();

            await using var command = CriarComando(
                connection,
                string.Empty);

            for (int i = 0; i < idsJogos.Count; i++)
            {
                var nomeParametro = $"@id{i}";

                parametrosIds.Add(nomeParametro);

                AdicionarParametro(
                    command,
                    nomeParametro,
                    idsJogos[i]);
            }

            command.CommandText = $@"
                SELECT
                    co_jogo,
                    co_genero
                FROM tb_jogo_genero
                WHERE co_jogo IN ({string.Join(", ", parametrosIds)})
                ORDER BY co_jogo, co_genero;
            ";

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                vinculos.Add(new JogoGenero
                {
                    IdJogo = reader.GetInt32("co_jogo"),
                    IdGenero = reader.GetInt32("co_genero")
                });
            }

            return vinculos;
        }

        // Insere os vínculos de um jogo com seus gêneros.
        public async Task InserirAsync(
            int idJogo,
            List<int> idsGeneros)
        {
            // Sem gêneros, não há vínculos para inserir.
            if (idsGeneros == null || idsGeneros.Count == 0)
                return;

            await using var connection = await AbrirConexaoAsync();

            // A transação garante que os vínculos sejam gravados
            // como uma unidade: todos ou nenhum.
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                const string sql = @"
                    INSERT INTO tb_jogo_genero (
                        co_jogo,
                        co_genero
                    )
                    VALUES (
                        @idJogo,
                        @idGenero
                    );
                ";

                foreach (var idGenero in idsGeneros)
                {
                    await using var command = CriarComando(connection, sql);

                    command.Transaction = transaction;

                    AdicionarParametro(command, "@idJogo", idJogo);
                    AdicionarParametro(command, "@idGenero", idGenero);

                    await command.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Remove os vínculos de um jogo.
        // O jogo em si não é removido.
        public async Task RemoverPorJogoAsync(int idJogo)
        {
            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                DELETE FROM tb_jogo_genero
                WHERE co_jogo = @idJogo;
            ";

            await using var command = CriarComando(connection, sql);

            AdicionarParametro(command, "@idJogo", idJogo);

            await ExecutarGravacaoAsync(command);
        }
    }
}
