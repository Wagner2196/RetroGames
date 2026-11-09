using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public interface ICondicaoJogoRepository
    {
        /// <summary>
        /// Lista todas as condições de jogo.
        /// </summary>
        Task<List<CondicaoJogo>> ObterTodosAsync();
    }
}