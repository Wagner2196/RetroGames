
using RetroGames.Model.Model;

namespace RetroGames.Repository
{
    // Define os métodos que o Repository de plataformas deve implementar.
    // A interface declara o contrato, mas não contém o SQL.
    public interface IPlataformaRepository
    {
        // Retorna todas as plataformas ordenadas pelo nome.
        Task<List<Plataforma>> ListarAsync();

        // Busca uma plataforma pelo seu identificador.
        // Retorna null caso não seja encontrada.
        Task<Plataforma?> BuscarPorIdAsync(int id);

        // Verifica se já existe uma plataforma com o nome informado.
        // O idIgnorar permite desconsiderar uma plataforma durante a alteração.
        Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null);

        // Verifica se algum jogo utiliza a plataforma informada.
        Task<bool> PossuiJogosAsync(int id);

        // Insere uma plataforma e retorna o identificador gerado pelo banco.
        Task<int> InserirAsync(Plataforma plataforma);

        // Altera os dados de uma plataforma existente.
        Task AlterarAsync(Plataforma plataforma);

        // Exclui uma plataforma pelo identificador.
        Task ExcluirAsync(int id);
    }
}
