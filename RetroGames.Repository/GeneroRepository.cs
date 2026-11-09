
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class GeneroRepository : RepositoryBase, IGeneroRepository
    {
        public GeneroRepository(DbConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<List<Genero>> ObterTodosAsync()
        {
            var generos = new List<Genero>();

            await using var connection = await AbrirConexaoAsync();

            const string sql = @"
                SELECT
                    co_seq_genero,
                    no_genero
                FROM tb_genero
                ORDER BY no_genero;
            ";

            await using var command = CriarComando(connection, sql);
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
