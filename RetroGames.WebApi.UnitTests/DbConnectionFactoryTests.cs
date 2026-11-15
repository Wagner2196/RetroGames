
using System;
using System.Collections.Generic;
using System.Linq;
using RetroGames.Repository;
using Xunit;

namespace RetroGames.WebApi.UnitTests
{
    // Classe responsável por testar a conexão com o banco
    // e o funcionamento dos repositórios.
    public class DbConnectionFactoryTests
    {
        // Testa se a aplicação consegue conectar ao MySQL.
        [Fact]
        public async Task CreateConnection_DeveConectarAoMySql()
        {
            var config = CriarConfig();
            var factory = new DbConnectionFactory(config);

            await using var connection = factory.CreateConnection();
            await connection.OpenAsync();

            Assert.Equal("RetroGames", connection.Database);
        }

        // Testa se o repositório consegue buscar as condições dos jogos.
        [Fact]
        public async Task ObterTodosAsync_DeveRetornarCondicoes()
        {
            var config = CriarConfig();
            var repository = new CondicaoJogoRepository(config);

            var condicoes = await repository.ObterTodosAsync();

            Assert.NotEmpty(condicoes);
            Assert.Contains(condicoes, c => c.Descricao == "Novo");
            Assert.Contains(condicoes, c => c.Descricao == "Usado");
        }

        // Testa se o repositório consegue buscar as plataformas.
        [Fact]
        public async Task PlataformaRepository_DeveRetornarPlataformas()
        {
            var config = CriarConfig();
            var repository = new PlataformaRepository(config);

            var plataformas = await repository.ListarAsync();

            Assert.NotEmpty(plataformas);
            Assert.Contains(plataformas, p => p.Nome == "PlayStation 5");
            Assert.Contains(plataformas, p => p.Nome == "Xbox Series X");
            Assert.Contains(plataformas, p => p.Nome == "Nintendo Switch");
        }

        // Testa se o repositório consegue buscar os gêneros.
        [Fact]
        public async Task GeneroRepository_DeveRetornarGeneros()
        {
            var config = CriarConfig();
            var repository = new GeneroRepository(config);

            var generos = await repository.ObterTodosAsync();

            Assert.NotEmpty(generos);
            Assert.Contains(generos, g => g.Nome == "Ação");
            Assert.Contains(generos, g => g.Nome == "Aventura");
            Assert.Contains(generos, g => g.Nome == "RPG");
        }

        // Testa se o repositório consegue listar os jogos.
        [Fact]
        public async Task JogoRepository_DeveRetornarJogos()
        {
            var config = CriarConfig();
            var repository = new JogoRepository(config);

            var resultado = await repository.ListarAsync(
                incluirInativos: true,
                pagina: 1,
                tamanhoPagina: 100);

            var jogos = resultado.Itens;

            Assert.NotEmpty(jogos);
            Assert.True(resultado.Total >= jogos.Count);

            Assert.Contains(jogos, j => j.Nome == "God of War Ragnarök");
            Assert.Contains(jogos, j => j.Nome == "Forza Horizon 5");
            Assert.Contains(jogos, j => j.Nome == "The Legend of Zelda: Tears of the Kingdom");
        }

        // Testa se o repositório consegue buscar os usuários.
        [Fact]
        public async Task UsuarioRepository_DeveRetornarUsuarios()
        {
            var config = CriarConfig();
            var factory = new DbConnectionFactory(config);
            var repository = new UsuarioRepository(factory);

            var usuarios = await repository.ObterTodosAsync();

            Assert.NotEmpty(usuarios);
            Assert.Contains(usuarios, u => u.Email == "cliente@retrogames.com");
        }

        // Testa se o repositório consegue buscar os perfis.
        [Fact]
        public async Task PerfilRepository_DeveRetornarPerfis()
        {
            var config = CriarConfig();
            var factory = new DbConnectionFactory(config);
            var repository = new PerfilRepository(factory);

            var perfis = await repository.ObterTodosAsync();

            Assert.NotEmpty(perfis);
            Assert.Contains(perfis, p => p.Descricao == "Administrador");
            Assert.Contains(perfis, p => p.Descricao == "Cliente");
        }

        // Testa se o repositório consegue listar os vínculos entre jogos e gêneros.
        [Fact]
        public async Task JogoGeneroRepository_DeveListarTodosOsVinculos()
        {
            var config = CriarConfig();
            var repository = new JogoGeneroRepository(config);

            var vinculos = await repository.ListarTodosAsync();

            Assert.NotEmpty(vinculos);
        }

        // Testa se é possível buscar os vínculos de uma lista específica de jogos.
        [Fact]
        public async Task JogoGeneroRepository_DeveListarVinculosPorJogos()
        {
            var config = CriarConfig();
            var repository = new JogoGeneroRepository(config);
            var jogoRepository = new JogoRepository(config);

            var resultadoJogos = await jogoRepository.ListarAsync(
                incluirInativos: true,
                pagina: 1,
                tamanhoPagina: 10);

            var idsJogos = resultadoJogos.Itens
                .Select(j => j.Id)
                .ToList();

            var vinculos = await repository.ListarPorJogosAsync(idsJogos);

            Assert.All(vinculos, v => Assert.Contains(v.IdJogo, idsJogos));
        }

        // Testa se é possível remover e inserir vínculos entre um jogo e gêneros.
        [Fact]
        public async Task JogoGeneroRepository_DeveInserirERemoverVinculos()
        {
            var config = CriarConfig();
            var repository = new JogoGeneroRepository(config);

            int idJogo = 1;
            var idsGeneros = new List<int> { 1, 3 };

            await repository.RemoverPorJogoAsync(idJogo);
            await repository.InserirAsync(idJogo, idsGeneros);

            var vinculos = await repository.ListarPorJogosAsync(
                new List<int> { idJogo });

            Assert.Equal(2, vinculos.Count);
            Assert.Contains(vinculos, v => v.IdGenero == 1);
            Assert.Contains(vinculos, v => v.IdGenero == 3);

            // Cleanup: remove os vínculos criados para não deixar alterações no banco.
            await repository.RemoverPorJogoAsync(idJogo);
        }

        // Cria a configuração de conexão utilizada pelos testes.
        private static RepositoryConfig CriarConfig()
        {
            // Carrega as configurações do arquivo .env.
            DotNetEnv.Env.TraversePath().Load();

            var connectionString =
                Environment.GetEnvironmentVariable("ConnectionStrings__Default");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "A variável ConnectionStrings__Default não foi configurada.");
            }

            return new RepositoryConfig
            {
                ConnectionString = connectionString
            };
        }
    }
}
