
using RetroGames.Model.Model;

namespace RetroGames.Services
{
    public interface IGeneroService
    {
        // Lista todos os gêneros.
        Task<List<Genero>> ObterTodosAsync();
    }
}
