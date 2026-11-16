
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlataformaController : ControllerBase
    {
        private readonly IPlataformaService _service;

        public PlataformaController(IPlataformaService service)
        {
            _service = service;
        }


        // Busca uma plataforma pelo ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> ListarPorId(int id)
        {
            var plataforma = await _service.BuscarPorIdAsync(id);

            if (plataforma == null)
                return NotFound();

            var response = new RetroGamesResponseApiModelT<ListarPlataformaResponseApiModel>
            {
                ResponseCode = ResponseCodes.SUCCESS,
                Data = plataforma.ParaResponse()
            };

            return Ok(response);
        }

        // Lista as plataformas e retorna o envelope padrão da API.
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var plataformas = await _service.ListarAsync();

            var response = new RetroGamesResponseApiModelT<List<ListarPlataformaResponseApiModel>>
            {
                ResponseCode = ResponseCodes.SUCCESS,
                Data = plataformas
                    .Select(plataforma => plataforma.ParaResponse())
                    .ToList()
            };

            return Ok(response);
        }
    }
}
