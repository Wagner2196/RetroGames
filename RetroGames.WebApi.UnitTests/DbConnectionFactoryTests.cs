
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
            var plataformas = await repository.ObterTodosAsync();

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
            var jogos = await repository.ObterTodosAsync();

            // Assert
            Assert.NotEmpty(jogos);
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
    }
}
