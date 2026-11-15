
using RetroGames.Model.Model;
using RetroGames.Repository;
using RetroGames.Services.Errors;

namespace RetroGames.Services
{
    // Esta classe contém as regras de negócio relacionadas às plataformas.
    public class PlataformaService : IPlataformaService
    {
        // Guarda o Repository responsável pelo acesso ao banco de dados.
        private readonly IPlataformaRepository _repository;

        // Recebe o Repository por injeção de dependência.
        public PlataformaService(IPlataformaRepository repository)
        {
            _repository = repository;
        }

        // Lista todas as plataformas cadastradas.
        public async Task<List<Plataforma>> ListarAsync()
        {
            return await _repository.ListarAsync();
        }


        // Mantém compatibilidade com o Controller antigo.
        // Encaminha a chamada para o novo método ListarAsync().
        public async Task<List<Plataforma>> ObterTodosAsync()
        {
            return await ListarAsync();
        }

        // Busca uma plataforma pelo ID.
        public async Task<Plataforma?> BuscarPorIdAsync(int id)
        {
            // Consulta o banco para encontrar a plataforma.
            var plataforma = await _repository.BuscarPorIdAsync(id);

            // Se não encontrar, lança um erro de negócio.
            if (plataforma == null)
            {
                throw new BusinessRuleException(
                    BusinessErrors.PlataformaNaoEncontrada);
            }

            return plataforma;
        }

        // Insere uma nova plataforma.
        public async Task<int> InserirAsync(Plataforma plataforma)
        {
            // Verifica se já existe uma plataforma com o mesmo nome.
            if (await _repository.ExisteComNomeAsync(plataforma.Nome))
            {
                throw new BusinessRuleException(
                    BusinessErrors.PlataformaNomeDuplicado);
            }

            try
            {
                // Se não houver duplicidade, realiza a inserção.
                return await _repository.InserirAsync(plataforma);
            }
            catch (RegistroDuplicadoException)
            {
                // Trata uma possível duplicidade identificada pelo banco.
                throw new BusinessRuleException(
                    BusinessErrors.PlataformaNomeDuplicado);
            }
        }

        // Altera os dados de uma plataforma existente.
        public async Task AlterarAsync(Plataforma plataforma)
        {
            // Primeiro verifica se a plataforma existe.
            await BuscarPorIdAsync(plataforma.Id);

            // Verifica se outra plataforma já utiliza o mesmo nome.
            if (await _repository.ExisteComNomeAsync(
                plataforma.Nome, plataforma.Id))
            {
                throw new BusinessRuleException(
                    BusinessErrors.PlataformaNomeDuplicado);
            }

            try
            {
                // Se as verificações passarem, atualiza os dados.
                await _repository.AlterarAsync(plataforma);
            }
            catch (RegistroDuplicadoException)
            {
                throw new BusinessRuleException(
                    BusinessErrors.PlataformaNomeDuplicado);
            }
        }

        // Exclui uma plataforma pelo ID.
        public async Task ExcluirAsync(int id)
        {
            // Verifica se a plataforma existe.
            await BuscarPorIdAsync(id);

            // Verifica se existem jogos vinculados a ela.
            if (await _repository.PossuiJogosAsync(id))
            {
                throw new BusinessRuleException(
                    BusinessErrors.PlataformaPossuiJogos);
            }

            // Só exclui quando todas as verificações forem aprovadas.
            await _repository.ExcluirAsync(id);
        }

    }

}
