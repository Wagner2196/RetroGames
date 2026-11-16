using RetroGames.Repository;
using RetroGames.WebApi.Configuration;
using RetroGames.WebApi.Middlewares;

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

// Registra os Controllers.
builder.Services.AddControllers();

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
