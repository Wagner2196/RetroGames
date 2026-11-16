
using RetroGames.WebApi.ApiModels;
using RetroGames.Services;

namespace RetroGames.WebApi.Mappers
{
    // Converte os dados de jogos para os modelos de resposta da API.
    public static class JogoMapper
    {
        // Converte os dados de um jogo para a listagem do catálogo.
        public static ListarJogoResponseApiModel ParaResponse(
            this JogoDetalheServiceModel detalhe)
        {
            return new ListarJogoResponseApiModel
            {
                Id = detalhe.Jogo.Id,
                Nome = detalhe.Jogo.Nome,
                AnoLancamento = detalhe.Jogo.AnoLancamento,
                Preco = detalhe.Jogo.Preco,
                Estoque = detalhe.Jogo.Estoque,
                Ativo = detalhe.Jogo.Ativo,
                NomePlataforma = detalhe.NomePlataforma,
                DescricaoCondicao = detalhe.DescricaoCondicao,
                Generos = detalhe.Generos
                    .Select(genero => genero.Nome)
                    .ToList()
            };
        }

        // Converte os dados de um jogo para a consulta detalhada.
        public static ListarPorIdJogoResponseApiModel ParaDetalheResponse(
            this JogoDetalheServiceModel detalhe)
        {
            return new ListarPorIdJogoResponseApiModel
            {
                Id = detalhe.Jogo.Id,
                Nome = detalhe.Jogo.Nome,
                Descricao = detalhe.Jogo.Descricao,
                AnoLancamento = detalhe.Jogo.AnoLancamento,
                Preco = detalhe.Jogo.Preco,
                Estoque = detalhe.Jogo.Estoque,
                Ativo = detalhe.Jogo.Ativo,
                IdPlataforma = detalhe.Jogo.IdPlataforma,
                NomePlataforma = detalhe.NomePlataforma,
                IdCondicaoJogo = detalhe.Jogo.IdCondicaoJogo,
                DescricaoCondicao = detalhe.DescricaoCondicao,
                Generos = detalhe.Generos
                    .Select(genero => new ListarGeneroResponseApiModel
                    {
                        Id = genero.Id,
                        Nome = genero.Nome
                    })
                    .ToList()
            };
        }
    }
}
