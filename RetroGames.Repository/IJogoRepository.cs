
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    // Define o contrato do Repository de jogos.
    // A interface declara quais operações devem existir,
    // mas não contém a implementação do SQL.
    public interface IJogoRepository
    {
        // Lista os jogos de forma paginada.
        //
        // nome: filtra jogos por parte do nome.
        // idPlataforma: filtra pela plataforma informada.
        // ids: filtra por uma lista de identificadores de jogos.
        // incluirInativos: permite incluir jogos desativados.
        // pagina: indica qual página será consultada.
        // tamanhoPagina: quantidade de jogos por página.
        //
        // Retorna os jogos da página e o total encontrado.
        Task<PaginaResultado<Jogo>> ListarAsync(
            string? nome = null,
            int? idPlataforma = null,
            List<int>? ids = null,
            bool incluirInativos = false,
            int pagina = 1,
            int tamanhoPagina = 10);

        // Busca um jogo pelo identificador.
        // Retorna null caso o jogo não seja encontrado.
        Task<Jogo?> BuscarPorIdAsync(int id);

        // Insere um novo jogo no banco de dados.
        // Retorna o identificador gerado pelo banco.
        Task<int> InserirAsync(Jogo jogo);

        // Atualiza os dados de um jogo existente.
        Task AlterarAsync(Jogo jogo);

        // Desativa um jogo logicamente.
        // O registro permanece no banco, mas st_ativo passa a ser 0.
        Task DesativarAsync(int id);
    }
}
