
using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public class UsuarioRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UsuarioRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            var usuarios = new List<Usuario>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT
                    co_seq_usuario,
                    no_usuario,
                    ds_email,
                    ds_senha_hash,
                    st_ativo,
                    dt_cadastro,
                    co_perfil
                FROM tb_usuario;
            ";

            await using var command = new MySqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var usuario = new Usuario
                {
                    Id = reader.GetInt32("co_seq_usuario"),
                    Nome = reader.GetString("no_usuario"),
                    Email = reader.GetString("ds_email"),
                    SenhaHash = reader.GetString("ds_senha_hash"),
                    Ativo = reader.GetBoolean("st_ativo"),
                    DataCadastro = reader.GetDateTime("dt_cadastro"),
                    IdPerfil = reader.GetInt32("co_perfil")
                };

                usuarios.Add(usuario);
            }

            return usuarios;
        }
    }
}
