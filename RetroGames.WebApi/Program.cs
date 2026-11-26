
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RetroGames.Repository;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.ActionFilters;
using RetroGames.WebApi.Configuration;
using RetroGames.WebApi.Middlewares;
using System.Text.Json;

// Carrega as variáveis do arquivo .env.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Obtém a string de conexão.
var connectionString =
    builder.Configuration["ConnectionStrings:Default"];

// Impede que a aplicação inicie sem a configuração do banco.
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "A configuração ConnectionStrings:Default não foi encontrada.");
}

// Registra a configuração do banco como Singleton.
builder.Services.AddSingleton(new RepositoryConfig
{
    ConnectionString = connectionString
});

// Registra os validators encontrados no assembly da API.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Registra os Controllers e adiciona o filtro de validação globalmente.
var mvcBuilder = builder.Services.AddControllers(opcoes =>
{
    opcoes.Filters.Add<FluentValidationFilter>();
});

// Personaliza as respostas automáticas de erro do ASP.NET Core.
mvcBuilder.ConfigureApiBehaviorOptions(opcoes =>
{
    opcoes.InvalidModelStateResponseFactory = contexto =>
    {
        // Reúne os erros de validação associados a cada campo.
        var errosPorCampo = contexto.ModelState
            .Where(item => item.Value?.Errors.Count > 0
                && !string.IsNullOrWhiteSpace(item.Key))
            .ToDictionary(
                item => JsonNamingPolicy.CamelCase.ConvertName(item.Key),
                item => item.Value!.Errors
                    .Select(erro => erro.ErrorMessage)
                    .ToArray()
            );

        // Se não houver erros associados a campos,
        // os dados serão enviados como nulos.
        object? dados = errosPorCampo.Count > 0
            ? errosPorCampo
            : null;

        // Devolve o erro no envelope padrão da API.
        return new BadRequestObjectResult(new
        {
            responseCode = ResponseCodes.MODEL_STATE_INVALIDO,
            data = dados,
            errorMessage = "Há um erro com o modelo enviado."
        });
    };
});

// Configura o Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra os Repositories.
builder.Services.AddRepositorios();

// Registra os Services.
builder.Services.AddServicosDeNegocio();

// Registra o tratamento global de exceções.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Ativa a resposta padrão para exceções não tratadas.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Ativa o tratamento global de exceções.
app.UseExceptionHandler();

// Configura o Swagger no ambiente de desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
