
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services
{
    // Esta classe contém a lógica de serviço relacionada às plataformas.
    public class PlataformaService
    {
        // Guarda uma referência ao repositório que acessa os dados.
        private readonly IPlataformaRepository _repository;

        // O construtor recebe o repositório por injeção de dependência.
        // Isso permite que o serviço utilize qualquer classe que implemente
        // a interface IPlataformaRepository.
        public PlataformaService(IPlataformaRepository repository)
        {
            // Armazena o repositório recebido para ser utilizado pelos métodos.
            _repository = repository;
        }

        // Método assíncrono que solicita ao repositório a lista de plataformas.
        // Task<List<Plataforma>> representa uma operação que retornará
        // uma lista de objetos Plataforma.
        public async Task<List<Plataforma>> ObterTodosAsync()
        {
            // Chama o método ListarAsync do repositório.
            // O await aguarda a consulta ao banco sem bloquear a execução.
            return await _repository.ListarAsync();
        }
    }
}
