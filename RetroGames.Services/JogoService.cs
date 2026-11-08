using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class JogoService
    {
        private readonly JogoRepository _repository;

        public JogoService(JogoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Jogo>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }
    }
}
