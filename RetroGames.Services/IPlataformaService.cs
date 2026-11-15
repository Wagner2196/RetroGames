
using RetroGames.Model.Model;

namespace RetroGames.Services
{
    // Define quais operações o serviço de plataformas deve oferecer.
    // A interface funciona como um contrato para a classe PlataformaService.
    public interface IPlataformaService
    {
        // Retorna a lista de todas as plataformas.
        Task<List<Plataforma>> ListarAsync();

        // Busca uma plataforma pelo seu ID.
        // Retorna null caso ela não seja encontrada.
        Task<Plataforma?> BuscarPorIdAsync(int id);

        // Cadastra uma nova plataforma.
        Task<int> InserirAsync(Plataforma plataforma);

        // Atualiza os dados de uma plataforma existente.
        Task AlterarAsync(Plataforma plataforma);

        // Exclui uma plataforma pelo seu ID.
        Task ExcluirAsync(int id);
    }
}
