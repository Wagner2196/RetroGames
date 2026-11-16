
using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers
{
    // Converte objetos de Plataforma para modelos de resposta da API.
    public static class PlataformaMapper
    {
        // Método de extensão que transforma Plataforma em resposta da API.
        public static ListarPlataformaResponseApiModel ParaResponse(
            this Plataforma plataforma)
        {
            return new ListarPlataformaResponseApiModel
            {
                Id = plataforma.Id,
                Nome = plataforma.Nome,
                Fabricante = plataforma.Fabricante,
                AnoLancamento = plataforma.AnoLancamento
            };
        }
    }
}
