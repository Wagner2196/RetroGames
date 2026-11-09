
namespace RetroGames.Repository
{
    // Classe responsável por armazenar as configurações
    // utilizadas pelos Repositories.
    public class RepositoryConfig
    {
        // Propriedade que guarda a string de conexão com o banco MySQL.
        // O valor será configurado posteriormente, a partir do arquivo .env.
        public string ConnectionString { get; set; } = string.Empty;
    }
}
