
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.ActionFilters
{
    // Filtro executado antes das actions dos controllers.
    // Sua responsabilidade é validar os dados recebidos na requisição.
    public class FluentValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // Procura um parâmetro da action que recebe dados no corpo da requisição.
            var parametroDoCorpo = context.ActionDescriptor.Parameters
                .FirstOrDefault(parametro =>
                    parametro.BindingInfo?.BindingSource == BindingSource.Body);

            // Se o corpo não foi enviado, devolve HTTP 400 com o código 2.
            if (parametroDoCorpo != null &&
                context.ActionArguments.TryGetValue(
                    parametroDoCorpo.Name, out var modelo) &&
                modelo == null)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    responseCode = ResponseCodes.MODEL_NULL,
                    data = (object?)null,
                    errorMessage = "O modelo veio nulo."
                });

                return;
            }

            // Percorre os argumentos recebidos pela action.
            foreach (var argumento in context.ActionArguments)
            {
                // Não há o que validar quando o argumento é nulo.
                if (argumento.Value == null)
                    continue;

                // Descobre se existe um validator registrado para o tipo do argumento.
                var tipoValidator = typeof(IValidator<>)
                    .MakeGenericType(argumento.Value.GetType());

                var validator = context.HttpContext.RequestServices
                    .GetService(tipoValidator) as IValidator;

                // Se não houver validator para esse tipo, passa para o próximo argumento.
                if (validator == null)
                    continue;

                // Executa as regras de validação do modelo.
                var contextoValidacao =
                    new ValidationContext<object>(argumento.Value);

                var resultado = await validator.ValidateAsync(contextoValidacao);

                // Se houver erros, organiza as mensagens por campo.
                if (!resultado.IsValid)
                {
                    var errosPorCampo = resultado.Errors
                        .GroupBy(erro => erro.PropertyName)
                        .ToDictionary(
                            grupo => JsonNamingPolicy.CamelCase.ConvertName(grupo.Key),
                            grupo => grupo.Select(erro => erro.ErrorMessage).ToArray()
                        );

                    // Interrompe a execução da action e devolve HTTP 400.
                    context.Result = new BadRequestObjectResult(new
                    {
                        responseCode = ResponseCodes.MODEL_STATE_INVALIDO,
                        data = errosPorCampo,
                        errorMessage = "Há um erro com o modelo enviado."
                    });

                    return;
                }
            }

            // Se não houver erros, permite que a action seja executada normalmente.
            await next();
        }
    }
}
