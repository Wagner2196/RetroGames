
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;

namespace RetroGames.WebApi.Controllers
{
    // Indica que esta classe é um Controller de uma API.
    [ApiController]

    // Define a rota: api/Jogo
    [Route("api/[controller]")]
    public class JogoController : ControllerBase
    {
        // Guarda o Service que será utilizado pelo Controller.
        private readonly JogoService _service;

        // O ASP.NET Core entrega o Service por injeção de dependência.
        public JogoController(JogoService service)
        {
            _service = service;
        }

        // Responde a requisições HTTP GET.
        // Permite filtrar e paginar os jogos.
        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] string? nome = null,
            [FromQuery] int? idPlataforma = null,
            [FromQuery] List<int>? ids = null,
            [FromQuery] bool incluirInativos = false,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            // Solicita ao Service os jogos e o total encontrado.
            var resultado = await _service.ListarAsync(
                nome,
                idPlataforma,
                ids,
                incluirInativos,
                pagina,
                tamanhoPagina);

            // Retorna o resultado com status HTTP 200 (OK).
            return Ok(resultado);
        }
    }
}
