
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CondicaoJogoController : ControllerBase
    {
        private readonly ICondicaoJogoService _service;

        public CondicaoJogoController(ICondicaoJogoService service)
        {
            _service = service;
        }

        // Lista todas as condições de jogo.
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var condicoes = await _service.ObterTodosAsync();

            var response =
                new RetroGamesResponseApiModelT<List<ListarCondicaoJogoResponseApiModel>>
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    Data = condicoes
                        .Select(condicao => condicao.ParaResponse())
                        .ToList()
                };

            return Ok(response);
        }
    }
}
