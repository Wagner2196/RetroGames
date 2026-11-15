
using RetroGames.Model.Model;
using RetroGames.Repository;
using RetroGames.Services.Errors;

namespace RetroGames.Services
{
    public class PlataformaService : IPlataformaService
    {
        // Recebe o conjunto de repositórios.
        private readonly IRepositorySet _repositorySet;

        // O construtor recebe o IRepositorySet.
        public PlataformaService(IRepositorySet repositorySet)
        {
            _repositorySet = repositorySet;
        }

        // Lista todas as plataformas.
        public async Task<List<Plataforma>> ListarAsync()
        {
            return await _repositorySet.Plataforma.ListarAsync();
        }

        // Mantém compatibilidade com o Controller antigo.
        public async Task<List<Plataforma>> ObterTodosAsync()
        {
            return await ListarAsync();
        }

        // Busca uma plataforma pelo ID.
        public async Task<Plataforma?> BuscarPorIdAsync(int id)
        {
            var plataforma = await _repositorySet.Plataforma.BuscarPorIdAsync(id);

            if (plataforma == null)
                throw new BusinessRuleException(BusinessErrors.PlataformaNaoEncontrada);

            return plataforma;
        }

        // Insere uma nova plataforma.
        public async Task<int> InserirAsync(Plataforma plataforma)
        {
            plataforma.Nome = plataforma.Nome.Trim();

            if (await _repositorySet.Plataforma.ExisteComNomeAsync(plataforma.Nome))
                throw new BusinessRuleException(BusinessErrors.PlataformaNomeDuplicado);

            try
            {
                return await _repositorySet.Plataforma.InserirAsync(plataforma);
            }
            catch (RegistroDuplicadoException)
            {
                throw new BusinessRuleException(BusinessErrors.PlataformaNomeDuplicado);
            }
        }

        // Altera uma plataforma existente.
        public async Task AlterarAsync(Plataforma plataforma)
        {
            await BuscarPorIdAsync(plataforma.Id);

            plataforma.Nome = plataforma.Nome.Trim();

            if (await _repositorySet.Plataforma.ExisteComNomeAsync(
                plataforma.Nome, plataforma.Id))
            {
                throw new BusinessRuleException(BusinessErrors.PlataformaNomeDuplicado);
            }

            try
            {
                await _repositorySet.Plataforma.AlterarAsync(plataforma);
            }
            catch (RegistroDuplicadoException)
            {
                throw new BusinessRuleException(BusinessErrors.PlataformaNomeDuplicado);
            }
        }

        // Exclui uma plataforma, desde que não possua jogos vinculados.
        public async Task ExcluirAsync(int id)
        {
            await BuscarPorIdAsync(id);

            if (await _repositorySet.Plataforma.PossuiJogosAsync(id))
                throw new BusinessRuleException(BusinessErrors.PlataformaPossuiJogos);

            await _repositorySet.Plataforma.ExcluirAsync(id);
        }
    }
}
