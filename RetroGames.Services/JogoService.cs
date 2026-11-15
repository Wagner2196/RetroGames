
using RetroGames.Model.Model;
using RetroGames.Repository;
using RetroGames.Services.Errors;
using System.Transactions;

namespace RetroGames.Services
{
    public class JogoService : IJogoService
    {
        // Recebe o conjunto de repositórios.
        private readonly IRepositorySet _repositorySet;

        // O construtor recebe o IRepositorySet.
        public JogoService(IRepositorySet repositorySet)
        {
            _repositorySet = repositorySet;
        }

        // Monta os detalhes dos jogos, incluindo plataforma,
        // condição e gêneros associados.
        private async Task<List<JogoDetalheServiceModel>> MontarDetalhesAsync(
            List<Jogo> jogos)
        {
            // Se não houver jogos, não precisamos consultar os outros repositórios.
            if (jogos.Count == 0)
                return new List<JogoDetalheServiceModel>();

            // Obtém os IDs dos jogos para buscar seus gêneros em uma única consulta.
            var idsJogos = jogos.Select(j => j.Id).ToList();

            // Busca os dados necessários para montar os detalhes.
            var plataformas = await _repositorySet.Plataforma.ListarAsync();
            var condicoes = await _repositorySet.CondicaoJogo.ObterTodosAsync();
            var generos = await _repositorySet.Genero.ObterTodosAsync();
            var vinculos = await _repositorySet.JogoGenero.ListarPorJogosAsync(idsJogos);

            // Organiza as plataformas por identificador.
            var plataformasPorId = plataformas.ToDictionary(p => p.Id);

            // Organiza as condições por identificador.
            var condicoesPorId = condicoes.ToDictionary(c => c.Id);

            // Relaciona cada jogo aos seus respectivos gêneros.
            var generosPorJogo = vinculos
                .Join(
                    generos,
                    vinculo => vinculo.IdGenero,
                    genero => genero.Id,
                    (vinculo, genero) => new
                    {
                        vinculo.IdJogo,
                        Genero = genero
                    })
                .GroupBy(x => x.IdJogo)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.Select(x => x.Genero).ToList());

            // Monta o resultado final com os dados completos de cada jogo.
            return jogos.Select(jogo => new JogoDetalheServiceModel
            {
                Jogo = jogo,

                NomePlataforma = plataformasPorId.TryGetValue(
                    jogo.IdPlataforma, out var plataforma)
                        ? plataforma.Nome
                        : string.Empty,

                DescricaoCondicao = condicoesPorId.TryGetValue(
                    jogo.IdCondicaoJogo, out var condicao)
                        ? condicao.Descricao
                        : string.Empty,

                Generos = generosPorJogo.TryGetValue(
                    jogo.Id, out var listaGeneros)
                        ? listaGeneros
                        : new List<Genero>()

            }).ToList();
        }

        // Lista os jogos com filtros e paginação.
        public async Task<PaginaResultado<JogoDetalheServiceModel>> ListarAsync(
            string? nome = null,
            int? idPlataforma = null,
            int? idGenero = null,
            bool incluirInativos = false,
            int pagina = 1,
            int tamanhoPagina = 10)
        {
            // Inicialmente, não aplicamos filtro por IDs de jogos.
            List<int>? idsJogos = null;

            // Se foi informado um gênero, precisamos descobrir quais jogos
            // estão associados a ele.
            if (idGenero.HasValue)
            {
                var vinculos = await _repositorySet.JogoGenero.ListarTodosAsync();

                idsJogos = vinculos
                    .Where(v => v.IdGenero == idGenero.Value)
                    .Select(v => v.IdJogo)
                    .Distinct()
                    .ToList();

                // Se nenhum jogo possuir esse gênero, retornamos uma página vazia.
                if (idsJogos.Count == 0)
                {
                    return new PaginaResultado<JogoDetalheServiceModel>
                    {
                        Itens = new List<JogoDetalheServiceModel>(),
                        Total = 0
                    };
                }
            }

            // Busca os jogos no Repository.
            // A paginação é realizada no SQL.
            var resultado = await _repositorySet.Jogo.ListarAsync(
                nome,
                idPlataforma,
                idsJogos,
                incluirInativos,
                pagina,
                tamanhoPagina);

            // Complementa os jogos com plataforma, condição e gêneros.
            var jogosDetalhados = await MontarDetalhesAsync(resultado.Itens);

            // Retorna os jogos detalhados mantendo o total informado pelo Repository.
            return new PaginaResultado<JogoDetalheServiceModel>
            {
                Itens = jogosDetalhados,
                Total = resultado.Total
            };
        }

        // Busca um jogo pelo identificador e retorna seus detalhes.
        public async Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id)
        {
            // Busca o jogo no banco de dados.
            var jogo = await _repositorySet.Jogo.BuscarPorIdAsync(id);

            // Verifica se o jogo foi encontrado.
            if (jogo == null)
                throw new BusinessRuleException(BusinessErrors.JogoNaoEncontrado);

            // Monta os detalhes do jogo encontrado.
            var jogosDetalhados = await MontarDetalhesAsync(new List<Jogo> { jogo });

            // Retorna o primeiro e único jogo encontrado.
            return jogosDetalhados.First();
        }

        // Insere um jogo e seus respectivos gêneros.
        public async Task<int> InserirAsync(
            Jogo jogo,
            List<int> idsGeneros)
        {
            // Verifica se a plataforma informada existe.
            var plataforma = await _repositorySet.Plataforma
                .BuscarPorIdAsync(jogo.IdPlataforma);

            if (plataforma == null)
                throw new BusinessRuleException(
                    BusinessErrors.PlataformaInexistente);

            // Verifica se a condição informada existe.
            var condicoes = await _repositorySet.CondicaoJogo
                .ObterTodosAsync();

            if (!condicoes.Any(c => c.Id == jogo.IdCondicaoJogo))
                throw new BusinessRuleException(
                    BusinessErrors.CondicaoNaoEncontrada);

            // Verifica se todos os gêneros informados existem.
            var generos = await _repositorySet.Genero.ObterTodosAsync();

            var idsGenerosExistentes = generos
                .Select(g => g.Id)
                .ToHashSet();

            if (idsGeneros.Any(id => !idsGenerosExistentes.Contains(id)))
                throw new BusinessRuleException(
                    BusinessErrors.GeneroNaoEncontrado);

            // Evita inserir o mesmo gênero mais de uma vez.
            idsGeneros = idsGeneros.Distinct().ToList();

            // Define os dados iniciais do novo jogo.
            jogo.Ativo = true;
            jogo.DataCadastro = DateTime.Now;

            // Configura as opções da transação.
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout,
            };

            // Inicia a transação para inserir o jogo e seus gêneros.
            using (var transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                transactionOptions,
                TransactionScopeAsyncFlowOption.Enabled))
            {
                // Insere o jogo e recupera o identificador gerado.
                jogo.Id = await _repositorySet.Jogo.InserirAsync(jogo);

                // Insere os vínculos entre o jogo e seus gêneros.
                await _repositorySet.JogoGenero.InserirAsync(
                    jogo.Id,
                    idsGeneros);

                // Confirma a transação.
                transactionScope.Complete();
            }

            // Retorna o identificador do jogo cadastrado.
            return jogo.Id;
        }


        public async Task AlterarAsync(Jogo jogo, List<int> idsGeneros)
        {
            // Verifica se o jogo existe.
            var jogoExistente = await _repositorySet.Jogo.BuscarPorIdAsync(jogo.Id);

            if (jogoExistente == null)
                throw new BusinessRuleException(BusinessErrors.JogoNaoEncontrado);

            // Verifica se a plataforma existe.
            var plataforma = await _repositorySet.Plataforma.BuscarPorIdAsync(jogo.IdPlataforma);

            if (plataforma == null)
                throw new BusinessRuleException(BusinessErrors.PlataformaInexistente);

            // Verifica se a condição do jogo existe.
            var condicoes = await _repositorySet.CondicaoJogo.ObterTodosAsync();

            if (!condicoes.Any(c => c.Id == jogo.IdCondicaoJogo))
                throw new BusinessRuleException(BusinessErrors.CondicaoNaoEncontrada);

            // Verifica se todos os gêneros existem.
            var generos = await _repositorySet.Genero.ObterTodosAsync();
            var idsGenerosExistentes = generos.Select(g => g.Id).ToHashSet();

            if (idsGeneros.Any(id => !idsGenerosExistentes.Contains(id)))
                throw new BusinessRuleException(BusinessErrors.GeneroNaoEncontrado);

            // Remove IDs repetidos.
            idsGeneros = idsGeneros.Distinct().ToList();

            // Preserva os dados que não devem ser alterados.
            jogo.Ativo = jogoExistente.Ativo;
            jogo.DataCadastro = jogoExistente.DataCadastro;

            // Configura a transação.
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout,
            };

            using (var transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                transactionOptions,
                TransactionScopeAsyncFlowOption.Enabled))
            {
                // Atualiza os dados do jogo.
                await _repositorySet.Jogo.AlterarAsync(jogo);

                // Remove os vínculos antigos.
                await _repositorySet.JogoGenero.RemoverPorJogoAsync(jogo.Id);

                // Insere os novos vínculos.
                await _repositorySet.JogoGenero.InserirAsync(jogo.Id, idsGeneros);

                transactionScope.Complete();
            }
        }

        public async Task DesativarAsync(int id)
        {
            // Verifica se o jogo existe.
            var jogo = await _repositorySet.Jogo.BuscarPorIdAsync(id);

            if (jogo == null)
                throw new BusinessRuleException(BusinessErrors.JogoNaoEncontrado);

            // Desativa o jogo no banco de dados.
            await _repositorySet.Jogo.DesativarAsync(id);
        }

    }
}
