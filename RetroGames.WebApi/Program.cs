
using RetroGames.Repository;
using RetroGames.WebApi.Configuration;

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

var app = builder.Build();

// Configura o Swagger no ambiente de desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
