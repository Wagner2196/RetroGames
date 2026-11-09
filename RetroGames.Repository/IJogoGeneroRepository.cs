
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    // Define as operações para administrar os vínculos
    // entre jogos e gêneros.
    public interface IJogoGeneroRepository
    {
        // Retorna todos os vínculos cadastrados.
        Task<List<JogoGenero>> ListarTodosAsync();

        // Retorna os vínculos relacionados a uma lista de jogos.
        // Todos os IDs são consultados em uma única operação.
        Task<List<JogoGenero>> ListarPorJogosAsync(List<int> idsJogos);

        // Grava os gêneros associados a um jogo.
        Task InserirAsync(int idJogo, List<int> idsGeneros);

        // Remove todos os vínculos de um jogo.
        // Usado antes de gravar novamente seus gêneros.
        Task RemoverPorJogoAsync(int idJogo);
    }
}
