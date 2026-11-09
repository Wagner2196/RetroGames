
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    public class JogoService
    {
        private readonly IJogoRepository _repository;

        // Recebe a interface do Repository.
        // Isso reduz o acoplamento com uma implementação específica.
        public JogoService(IJogoRepository repository)
        {
            _repository = repository;
        }

        // Encaminha os filtros e os dados de paginação
        // para o Repository e retorna o resultado.
        public async Task<PaginaResultado<Jogo>> ListarAsync(
            string? nome = null,
            int? idPlataforma = null,
            List<int>? ids = null,
            bool incluirInativos = false,
            int pagina = 1,
            int tamanhoPagina = 10)
        {
            return await _repository.ListarAsync(
                nome,
                idPlataforma,
                ids,
                incluirInativos,
                pagina,
                tamanhoPagina);
        }
    }
}
