
using RetroGames.Repository;
using Xunit;

namespace RetroGames.WebApi.UnitTests
{
    public class DbConnectionFactoryTests
    {
        [Fact]
        public async Task CreateConnection_DeveConectarAoMySql()
        {
            // Arrange
            var factory = new DbConnectionFactory();

            // Act
            await using var connection = factory.CreateConnection();
            await connection.OpenAsync();

            // Assert
            Assert.Equal("RetroGames", connection.Database);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarCondicoes()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new CondicaoJogoRepository(factory);

            // Act
            var condicoes = await repository.ObterTodosAsync();

            // Assert
            Assert.NotEmpty(condicoes);
            Assert.Contains(condicoes, c => c.Descricao == "Novo");
            Assert.Contains(condicoes, c => c.Descricao == "Usado");
        }

        [Fact]
        public async Task PlataformaRepository_DeveRetornarPlataformas()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new PlataformaRepository(factory);

            // Act
            var plataformas = await repository.ListarAsync();

            // Assert
            Assert.NotEmpty(plataformas);
            Assert.Contains(plataformas, p => p.Nome == "PlayStation 5");
            Assert.Contains(plataformas, p => p.Nome == "Xbox Series X");
            Assert.Contains(plataformas, p => p.Nome == "Nintendo Switch");
        }

        [Fact]
        public async Task GeneroRepository_DeveRetornarGeneros()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new GeneroRepository(factory);

            // Act
            var generos = await repository.ObterTodosAsync();

            // Assert
            Assert.NotEmpty(generos);
            Assert.Contains(generos, g => g.Nome == "Ação");
            Assert.Contains(generos, g => g.Nome == "Aventura");
            Assert.Contains(generos, g => g.Nome == "RPG");
        }
        [Fact]
        public async Task JogoRepository_DeveRetornarJogos()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new JogoRepository(factory);

            // Act
            var resultado = await repository.ListarAsync(
                incluirInativos: true,
                pagina: 1,
                tamanhoPagina: 100);

            var jogos = resultado.Itens;

            // Assert
            Assert.NotEmpty(jogos);
            Assert.True(resultado.Total >= jogos.Count);

            Assert.Contains(jogos, j => j.Nome == "God of War Ragnarök");
            Assert.Contains(jogos, j => j.Nome == "Forza Horizon 5");
            Assert.Contains(jogos, j => j.Nome == "The Legend of Zelda: Tears of the Kingdom");
        }

        [Fact]
        public async Task UsuarioRepository_DeveRetornarUsuarios()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new UsuarioRepository(factory);

            // Act
            var usuarios = await repository.ObterTodosAsync();

            // Assert
            Assert.NotEmpty(usuarios);
            Assert.Contains(usuarios, u => u.Email == "cliente@retrogames.com");
        }

        [Fact]
        public async Task PerfilRepository_DeveRetornarPerfis()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new PerfilRepository(factory);

            // Act
            var perfis = await repository.ObterTodosAsync();

            // Assert
            Assert.NotEmpty(perfis);
            Assert.Contains(perfis, p => p.Descricao == "Administrador");
            Assert.Contains(perfis, p => p.Descricao == "Cliente");
        }

        [Fact]
        public async Task JogoGeneroRepository_DeveListarTodosOsVinculos()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new JogoGeneroRepository(factory);

            // Act
            var vinculos = await repository.ListarTodosAsync();

            // Assert
            Assert.NotEmpty(vinculos);
        }
        [Fact]
        public async Task JogoGeneroRepository_DeveListarVinculosPorJogos()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new JogoGeneroRepository(factory);
            var jogoRepository = new JogoRepository(factory);

            var resultadoJogos = await jogoRepository.ListarAsync(
                incluirInativos: true,
                pagina: 1,
                tamanhoPagina: 10);

            var idsJogos = resultadoJogos.Itens
                .Select(j => j.Id)
                .ToList();

            // Act
            var vinculos = await repository.ListarPorJogosAsync(idsJogos);

            // Assert
            Assert.All(vinculos, v => Assert.Contains(v.IdJogo, idsJogos));
        }
        [Fact]
        public async Task JogoGeneroRepository_DeveInserirERemoverVinculos()
        {
            // Arrange
            var factory = new DbConnectionFactory();
            var repository = new JogoGeneroRepository(factory);

            int idJogo = 1;
            var idsGeneros = new List<int> { 1, 3 };

            // Act
            await repository.RemoverPorJogoAsync(idJogo);
            await repository.InserirAsync(idJogo, idsGeneros);

            var vinculos = await repository.ListarPorJogosAsync(
                new List<int> { idJogo });

            // Assert
            Assert.Equal(2, vinculos.Count);
            Assert.Contains(vinculos, v => v.IdGenero == 1);
            Assert.Contains(vinculos, v => v.IdGenero == 3);

            // Cleanup
            await repository.RemoverPorJogoAsync(idJogo);
        }
    }
}
