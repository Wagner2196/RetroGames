
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;

namespace RetroGames.WebApi.Controllers
{
    // Indica que esta classe é um Controller de uma API.
    [ApiController]

    // Define a rota: api/Usuario
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        // Guarda o Service que será utilizado pelo Controller.
        private readonly UsuarioService _service;

        // O ASP.NET Core entrega o Service por injeção de dependência.
        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        // Indica que este método responde a requisições HTTP GET.
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            // Solicita ao Service a lista de usuários cadastrados.
            var usuarios = await _service.ObterTodosAsync();

            // Retorna a lista com o status HTTP 200 (OK).
            return Ok(usuarios);
        }
    }
}
