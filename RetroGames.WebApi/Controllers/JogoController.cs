
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;

namespace RetroGames.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly JogoService _service;

        public JogoController(JogoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] string? nome = null,
            [FromQuery] int? idPlataforma = null,
            [FromQuery] int? idGenero = null,
            [FromQuery] bool incluirInativos = false,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var resultado = await _service.ListarAsync(
                nome,
                idPlataforma,
                idGenero,
                incluirInativos,
                pagina,
                tamanhoPagina);

            return Ok(resultado);
        }
    }
}
