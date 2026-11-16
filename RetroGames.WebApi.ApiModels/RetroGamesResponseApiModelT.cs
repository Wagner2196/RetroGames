
namespace RetroGames.WebApi.ApiModels
{
    public class RetroGamesResponseApiModelT<T>// O T indica que a classe recebe  um tipo de dado como parâmetro 
    {
        public int ResponseCode { get; set; }

        public string? ErrorMessage { get; set; }

        public T? Data { get; set; }
    }
}
