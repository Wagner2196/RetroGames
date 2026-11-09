
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    public interface IGeneroRepository
    {
        /// <summary>
        /// Lista todos os gêneros ordenados pelo nome.
        /// </summary>
        Task<List<Genero>> ObterTodosAsync();
    }
}
