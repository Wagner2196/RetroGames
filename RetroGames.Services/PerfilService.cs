using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class PerfilService
    {
        private readonly PerfilRepository _repository;

        public PerfilService(PerfilRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Perfil>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }
    }
}
