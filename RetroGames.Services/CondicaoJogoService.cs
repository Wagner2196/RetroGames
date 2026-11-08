using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class CondicaoJogoService
    {
        private readonly CondicaoJogoRepository _repository;

        public CondicaoJogoService(CondicaoJogoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CondicaoJogo>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }
    }
}
