
using RetroGames.Model.Model;
using RetroGames.Repository;
using RetroGames.Services;
using RetroGames.Services.Errors;

namespace RetroGames.WebApi.UnitTests;

public class PlataformaServiceTests
{
    [Fact]
    public async Task BuscarPorIdAsync_DeveRetornarPlataforma_QuandoExistir()
    {
        // Arrange
        var plataforma = new Plataforma
        {
            Id = 1,
            Nome = "PlayStation 5",
            Fabricante = "Sony",
            AnoLancamento = 2020
        };

        var repository = new FakePlataformaRepository();
        repository.Plataformas.Add(plataforma);

        var service = new PlataformaService(repository);

        // Act
        var resultado = await service.BuscarPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("PlayStation 5", resultado.Nome);
    }

    [Fact]
    public async Task BuscarPorIdAsync_DeveLancarExcecao_QuandoNaoExistir()
    {
        // Arrange
        var repository = new FakePlataformaRepository();
        var service = new PlataformaService(repository);

        // Act
        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.BuscarPorIdAsync(999)
        );

        // Assert
        Assert.Equal(BusinessErrors.PlataformaNaoEncontrada, excecao.Erro);
    }

    [Fact]
    public async Task InserirAsync_DeveCadastrarPlataforma_QuandoNomeNaoExistir()
    {
        // Arrange
        var repository = new FakePlataformaRepository();
        var service = new PlataformaService(repository);

        var plataforma = new Plataforma
        {
            Nome = "Nintendo Switch",
            Fabricante = "Nintendo",
            AnoLancamento = 2017
        };

        // Act
        var id = await service.InserirAsync(plataforma);

        // Assert
        Assert.True(id > 0);
        Assert.Single(repository.Plataformas);
        Assert.Equal("Nintendo Switch", repository.Plataformas[0].Nome);
    }

    [Fact]
    public async Task InserirAsync_DeveLancarExcecao_QuandoNomeJaExistir()
    {
        // Arrange
        var repository = new FakePlataformaRepository();

        repository.Plataformas.Add(new Plataforma
        {
            Id = 1,
            Nome = "PlayStation 5",
            Fabricante = "Sony",
            AnoLancamento = 2020
        });

        var service = new PlataformaService(repository);

        var novaPlataforma = new Plataforma
        {
            Nome = "PlayStation 5",
            Fabricante = "Sony",
            AnoLancamento = 2020
        };

        // Act
        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.InserirAsync(novaPlataforma)
        );

        // Assert
        Assert.Equal(BusinessErrors.PlataformaNomeDuplicado, excecao.Erro);
        Assert.Single(repository.Plataformas);
    }
}

// Repositório falso: simula o repositório real sem acessar o banco.
public class FakePlataformaRepository : IPlataformaRepository
{
    public List<Plataforma> Plataformas { get; } = new();

    public Task<List<Plataforma>> ListarAsync()
        => Task.FromResult(Plataformas);

    public Task<Plataforma?> BuscarPorIdAsync(int id)
        => Task.FromResult(Plataformas.FirstOrDefault(p => p.Id == id));

    public Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null)
        => Task.FromResult(Plataformas.Any(p =>
            p.Nome == nome && p.Id != idIgnorar));

    public Task<bool> PossuiJogosAsync(int id)
        => Task.FromResult(false);

    public Task<int> InserirAsync(Plataforma plataforma)
    {
        plataforma.Id = Plataformas.Count + 1;
        Plataformas.Add(plataforma);

        return Task.FromResult(plataforma.Id);
    }

    public Task AlterarAsync(Plataforma plataforma)
    {
        var existente = Plataformas.FirstOrDefault(p => p.Id == plataforma.Id);

        if (existente != null)
        {
            existente.Nome = plataforma.Nome;
            existente.Fabricante = plataforma.Fabricante;
            existente.AnoLancamento = plataforma.AnoLancamento;
        }

        return Task.CompletedTask;
    }

    public Task ExcluirAsync(int id)
    {
        var plataforma = Plataformas.FirstOrDefault(p => p.Id == id);

        if (plataforma != null)
            Plataformas.Remove(plataforma);

        return Task.CompletedTask;
    }
}
