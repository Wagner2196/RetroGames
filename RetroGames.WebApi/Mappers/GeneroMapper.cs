
using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers
{
    // Converte objetos de Genero para modelos de resposta da API.
    public static class GeneroMapper
    {
        // Transforma Genero em resposta da API.
        public static ListarGeneroResponseApiModel ParaResponse(
            this Genero genero)
        {
            return new ListarGeneroResponseApiModel
            {
                Id = genero.Id,
                Nome = genero.Nome
            };
        }
    }
}
