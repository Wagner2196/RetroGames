
using Microsoft.Extensions.DependencyInjection;
using RetroGames.Repository;
using RetroGames.Services;

namespace RetroGames.WebApi.Configuration
{
    public static class InjecaoDependenciaConfig
    {
        // Registra as dependências da camada Repository.
        public static IServiceCollection AddRepositorios(
            this IServiceCollection services)
        {
            // Fábrica de conexões.
            services.AddScoped<DbConnectionFactory>();

            // Repositórios concretos.
            services.AddScoped<CondicaoJogoRepository>();
            services.AddScoped<PlataformaRepository>();
            services.AddScoped<GeneroRepository>();
            services.AddScoped<JogoRepository>();
            services.AddScoped<JogoGeneroRepository>();
            services.AddScoped<UsuarioRepository>();
            services.AddScoped<PerfilRepository>();

            // Interfaces compartilhando a instância dos repositórios concretos.
            services.AddScoped<ICondicaoJogoRepository>(
                provider => provider.GetRequiredService<CondicaoJogoRepository>());

            services.AddScoped<IPlataformaRepository>(
                provider => provider.GetRequiredService<PlataformaRepository>());

            services.AddScoped<IGeneroRepository>(
                provider => provider.GetRequiredService<GeneroRepository>());

            services.AddScoped<IJogoRepository>(
                provider => provider.GetRequiredService<JogoRepository>());

            services.AddScoped<IJogoGeneroRepository>(
                provider => provider.GetRequiredService<JogoGeneroRepository>());

            // Conjunto de repositórios.
            services.AddScoped<IRepositorySet, RepositorySet>();

            return services;
        }

        // Registra as dependências da camada Services.
        public static IServiceCollection AddServicosDeNegocio(
            this IServiceCollection services)
        {
            // Serviços concretos.
            services.AddScoped<CondicaoJogoService>();
            services.AddScoped<GeneroService>();
            services.AddScoped<JogoService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<PerfilService>();
            services.AddScoped<PlataformaService>();

            // Interfaces compartilhando a instância dos serviços concretos.
            services.AddScoped<IGeneroService>(
                provider => provider.GetRequiredService<GeneroService>());

            services.AddScoped<ICondicaoJogoService>(
                provider => provider.GetRequiredService<CondicaoJogoService>());

            services.AddScoped<IPlataformaService>(
                provider => provider.GetRequiredService<PlataformaService>());

            services.AddScoped<IJogoService>(
                provider => provider.GetRequiredService<JogoService>());

            return services;
        }
    }
}
