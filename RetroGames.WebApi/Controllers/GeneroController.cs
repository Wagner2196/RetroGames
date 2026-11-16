
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeneroController : ControllerBase
    {
        private readonly IGeneroService _service;

        public GeneroController(IGeneroService service)
        {
            _service = service;
        }

        // Lista todos os gêneros.
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var generos = await _service.ObterTodosAsync();

            var response =
                new RetroGamesResponseApiModelT<List<ListarGeneroResponseApiModel>>
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    Data = generos
                        .Select(genero => genero.ParaResponse())
                        .ToList()
                };

            return Ok(response);
        }
    }
}
