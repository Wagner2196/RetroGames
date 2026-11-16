
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly IJogoService _service;

        public JogoController(IJogoService service)
        {
            _service = service;
        }

        // Lista os jogos com filtros e paginação.
        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] ListarJogoRequestApiModel request)
        {
            var resultado = await _service.ListarAsync(
                nome: request.Busca,
                idPlataforma: request.IdPlataforma,
                idGenero: request.IdGenero,
                pagina: request.Pagina,
                tamanhoPagina: request.TamanhoPagina);

            var paginaResponse =
                new PaginaResponseApiModel<ListarJogoResponseApiModel>
                {
                    Itens = resultado.Itens
                        .Select(jogo => jogo.ParaResponse())
                        .ToList(),

                    Pagina = request.Pagina,
                    TamanhoPagina = request.TamanhoPagina,
                    TotalItens = resultado.Total,

                    TotalPaginas = (int)Math.Ceiling(
                        (double)resultado.Total / request.TamanhoPagina)
                };

            var response =
                new RetroGamesResponseApiModelT<PaginaResponseApiModel<ListarJogoResponseApiModel>>
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    Data = paginaResponse
                };

            return Ok(response);
        }

        // Busca um jogo pelo ID, incluindo seus detalhes.
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var jogo = await _service.BuscarPorIdAsync(id);

            var response =
                new RetroGamesResponseApiModelT<ListarPorIdJogoResponseApiModel>
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    Data = jogo.ParaDetalheResponse()
                };

            return Ok(response);
        }
    }
}
