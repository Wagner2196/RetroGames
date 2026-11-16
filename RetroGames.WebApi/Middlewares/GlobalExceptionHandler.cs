
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using RetroGames.Services.Errors;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Middlewares
{
    // Centraliza o tratamento de exceções da API.
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger; //injeção do Ilogger

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Trata erros conhecidos de regras de negócio.
            if (exception is BusinessRuleException businessException)
            {
                httpContext.Response.StatusCode =
                    (int)businessException.Erro.StatusHttp;

                var businessResponse = new RetroGamesResponseApiModel
                {
                    ResponseCode = businessException.Erro.Codigo,
                    ErrorMessage = businessException.Message
                };

                await httpContext.Response.WriteAsJsonAsync(
                    businessResponse,
                    cancellationToken);

                return true;
            }

            // Registra os detalhes completos de erros inesperados.
            _logger.LogError(
                exception,
                "Ocorreu uma exceção inesperada na API.");

            // Retorna uma mensagem genérica ao cliente.
            httpContext.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            var response = new RetroGamesResponseApiModel
            {
                ResponseCode = ResponseCodes.ERROR,
                ErrorMessage = "Ocorreu um erro interno no servidor."
            };

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken);

            return true;
        }
    }
}
