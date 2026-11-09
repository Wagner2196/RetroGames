
using RetroGames.Repository;
using RetroGames.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte aos Controllers da API.
builder.Services.AddControllers();

// Configura o Swagger para documentar e testar os endpoints.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra a fábrica responsável por criar conexões com o MySQL.
builder.Services.AddScoped<DbConnectionFactory>();

// Registra os Repositories concretos.
builder.Services.AddScoped<CondicaoJogoRepository>();
builder.Services.AddScoped<PlataformaRepository>();
builder.Services.AddScoped<GeneroRepository>();
builder.Services.AddScoped<JogoRepository>();
builder.Services.AddScoped<JogoGeneroRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<PerfilRepository>();

// Registra as interfaces dos Repositories.
// A interface e a classe concreta compartilham a mesma instância
// durante o escopo da requisição.
builder.Services.AddScoped<ICondicaoJogoRepository>(
    provider => provider.GetRequiredService<CondicaoJogoRepository>());

builder.Services.AddScoped<IPlataformaRepository>(
    provider => provider.GetRequiredService<PlataformaRepository>());

builder.Services.AddScoped<IGeneroRepository>(
    provider => provider.GetRequiredService<GeneroRepository>());

builder.Services.AddScoped<IJogoRepository>(
    provider => provider.GetRequiredService<JogoRepository>());

builder.Services.AddScoped<IJogoGeneroRepository>(
    provider => provider.GetRequiredService<JogoGeneroRepository>());

// Registra o conjunto de Repositories.
builder.Services.AddScoped<IRepositorySet, RepositorySet>();

// Registra os Services.
builder.Services.AddScoped<CondicaoJogoService>();
builder.Services.AddScoped<PlataformaService>();
builder.Services.AddScoped<GeneroService>();
builder.Services.AddScoped<JogoService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<PerfilService>();

var app = builder.Build();

// Configura o Swagger apenas no ambiente de desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona requisições HTTP para HTTPS quando configurado.
app.UseHttpsRedirection();

// Permite que os Controllers recebam requisições.
app.MapControllers();

app.Run();
