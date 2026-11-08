using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _repository;

        public UsuarioService(UsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }
    }
}
