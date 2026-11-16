
namespace RetroGames.WebApi.ApiModels
{
    // Parâmetros utilizados para filtrar e paginar a listagem de jogos.
    public class ListarJogoRequestApiModel
    {
        public string? Busca { get; set; }
        public int? IdPlataforma { get; set; }
        public int? IdGenero { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
    }
}
