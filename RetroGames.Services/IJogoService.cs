
using RetroGames.Model.Model;

namespace RetroGames.Services
{
    public interface IJogoService
    {
        // Lista os jogos com filtros e paginação.
        Task<PaginaResultado<JogoDetalheServiceModel>> ListarAsync(
            string? nome = null,
            int? idPlataforma = null,
            int? idGenero = null,
            bool incluirInativos = false,
            int pagina = 1,
            int tamanhoPagina = 10);

        // Busca um jogo pelo ID, incluindo seus detalhes.
        Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);

        // Insere um jogo e seus gêneros.
        Task<int> InserirAsync(Jogo jogo, List<int> idsGeneros);

        // Altera um jogo e seus gêneros.
        Task AlterarAsync(Jogo jogo, List<int> idsGeneros);

        // Desativa um jogo sem excluir seu registro.
        Task DesativarAsync(int id);
    }
}
