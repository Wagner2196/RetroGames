
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class PlataformaService
    {
        private readonly PlataformaRepository _repository;

        public PlataformaService(PlataformaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Plataforma>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }
    }
}
