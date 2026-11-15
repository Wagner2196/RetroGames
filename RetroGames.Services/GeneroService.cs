
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class GeneroService : IGeneroService
    {
        // Recebe o conjunto de repositórios.
        private readonly IRepositorySet _repositorySet;

        // O construtor recebe o IRepositorySet.
        public GeneroService(IRepositorySet repositorySet)
        {
            _repositorySet = repositorySet;
        }

        // Lista todos os gêneros.
        public async Task<List<Genero>> ObterTodosAsync()
        {
            return await _repositorySet.Genero.ObterTodosAsync();
        }
    }
}
