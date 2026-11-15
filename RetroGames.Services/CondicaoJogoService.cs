
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class CondicaoJogoService : ICondicaoJogoService
    {
        // Recebe o conjunto de repositórios.
        private readonly IRepositorySet _repositorySet;

        // O construtor recebe o IRepositorySet.
        public CondicaoJogoService(IRepositorySet repositorySet)
        {
            _repositorySet = repositorySet;
        }

        // Lista todas as condições de jogo.
        public async Task<List<CondicaoJogo>> ObterTodosAsync()
        {
            return await _repositorySet.CondicaoJogo.ObterTodosAsync();
        }
    }
}
