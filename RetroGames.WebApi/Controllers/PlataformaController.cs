
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;

namespace RetroGames.WebApi.Controllers
{
    // Indica que esta classe é um Controller de uma API.
    [ApiController]

    // Define a rota: api/Plataforma
    [Route("api/[controller]")]
    public class PlataformaController : ControllerBase
    {
        // Guarda o Service que será utilizado pelo Controller.
        private readonly PlataformaService _service;

        // O ASP.NET Core entrega o Service por injeção de dependência.
        public PlataformaController(PlataformaService service)
        {
            _service = service;
        }

        // Indica que este método responde a requisições HTTP GET.
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            // Solicita ao Service a lista de plataformas cadastradas.
            var plataformas = await _service.ObterTodosAsync();

            // Retorna a lista com o status HTTP 200 (OK).
            return Ok(plataformas);
        }
    }
}
