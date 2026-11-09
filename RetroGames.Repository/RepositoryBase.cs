
using MySqlConnector;

namespace RetroGames.Repository
{
    public abstract class RepositoryBase
    {
        private readonly Func<MySqlConnection> _criarConexao;

        protected RepositoryBase(RepositoryConfig config)
        {
            _criarConexao = () =>
                new MySqlConnection(config.ConnectionString);
        }

        protected RepositoryBase(DbConnectionFactory connectionFactory)
        {
            _criarConexao = connectionFactory.CreateConnection;
        }

        protected async Task<MySqlConnection> AbrirConexaoAsync()
        {
            var connection = _criarConexao();

            try
            {
                await connection.OpenAsync();
                return connection;
            }
            catch
            {
                await connection.DisposeAsync();
                throw;
            }
        }

        protected MySqlCommand CriarComando(
            MySqlConnection connection,
            string sql)
        {
            return new MySqlCommand(sql, connection);
        }

        protected void AdicionarParametro(
            MySqlCommand command,
            string nome,
            object? valor)
        {
            command.Parameters.AddWithValue(
                nome,
                valor ?? DBNull.Value);
        }

        protected async Task<int> ExecutarGravacaoAsync(
            MySqlCommand command)
        {
            try
            {
                return await command.ExecuteNonQueryAsync();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                throw new RegistroDuplicadoException(
                    "Já existe um registro com esse valor único.");
            }
        }
    }
}
