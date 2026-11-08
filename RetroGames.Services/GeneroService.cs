
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class GeneroService
    {
        private readonly GeneroRepository _repository;

        public GeneroService(GeneroRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Genero>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }
    }
}
