
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;

namespace RetroGames.WebApi.Controllers
{
    // Indica que esta classe é um Controller de uma API.
    [ApiController]

    // Define a rota: api/CondicaoJogo
    [Route("api/[controller]")]
    public class CondicaoJogoController : ControllerBase
    {
        // Guarda o Service que será utilizado pelo Controller.
        private readonly CondicaoJogoService _service;

        // O ASP.NET Core entrega o Service por injeção de dependência.
        public CondicaoJogoController(CondicaoJogoService service)
        {
            _service = service;
        }

        // Indica que este método responde a requisições HTTP GET.
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            // Solicita ao Service a lista de condições cadastradas.
            var condicoes = await _service.ObterTodosAsync();

            // Retorna a lista com o status HTTP 200 (OK).
            return Ok(condicoes);
        }
    }
}
