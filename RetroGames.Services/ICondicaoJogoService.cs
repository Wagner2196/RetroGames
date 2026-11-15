
using RetroGames.Model.Model;

namespace RetroGames.Services
{
    public interface ICondicaoJogoService
    {
        // Lista todas as condições de jogo.
        Task<List<CondicaoJogo>> ObterTodosAsync();
    }
}
