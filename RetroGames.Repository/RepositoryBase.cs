
using MySqlConnector;

namespace RetroGames.Repository
{
    // Classe base abstrata para os Repositories.
    // Centraliza operações comuns de acesso ao banco de dados.
    public abstract class RepositoryBase
    {
        // Guarda as configurações necessárias para conectar ao banco.
        private readonly RepositoryConfig _config;

        // Recebe a configuração no momento em que a classe derivada é criada.
        protected RepositoryBase(RepositoryConfig config)
        {
            _config = config;
        }

        // Cria uma conexão com o MySQL e abre essa conexão.
        // A conexão é devolvida aberta para que o Repository possa utilizá-la.
        protected async Task<MySqlConnection> AbrirConexaoAsync()
        {
            var connection = new MySqlConnection(_config.ConnectionString);

            await connection.OpenAsync();

            return connection;
        }

        // Cria um comando SQL associado à conexão informada.
        // O SQL é recebido como parâmetro para que cada Repository
        // possa executar sua própria consulta.
        protected MySqlCommand CriarComando(
            MySqlConnection connection,
            string sql)
        {
            return new MySqlCommand(sql, connection);
        }

        // Adiciona um parâmetro ao comando SQL.
        // O uso de parâmetros ajuda a evitar SQL Injection.
        // Se o valor for nulo, ele será convertido para DBNull.Value.
        protected void AdicionarParametro(
            MySqlCommand command,
            string nome,
            object? valor)
        {
            command.Parameters.AddWithValue(
                nome,
                valor ?? DBNull.Value);
        }

        // Executa um comando de gravação, como INSERT ou UPDATE.
        // Se ocorrer o erro MySQL 1062, lança a exceção própria
        // RegistroDuplicadoException.
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
