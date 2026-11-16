
using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers
{
    // Converte objetos de CondicaoJogo para modelos de resposta da API.
    public static class CondicaoJogoMapper
    {
        // Transforma CondicaoJogo em resposta da API.
        public static ListarCondicaoJogoResponseApiModel ParaResponse(
            this CondicaoJogo condicaoJogo)
        {
            return new ListarCondicaoJogoResponseApiModel
            {
                Id = condicaoJogo.Id,
                Descricao = condicaoJogo.Descricao
            };
        }
    }
}
