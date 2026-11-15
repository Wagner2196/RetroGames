
using RetroGames.Model.Model;

namespace RetroGames.Services
{
    public class JogoDetalheServiceModel
    {
        // Dados principais do jogo.
        public Jogo Jogo { get; set; } = new();

        // Nome da plataforma do jogo.
        public string NomePlataforma { get; set; } = string.Empty;

        // Descrição da condição do jogo.
        public string DescricaoCondicao { get; set; } = string.Empty;

        // Lista de gêneros associados ao jogo.
        public List<Genero> Generos { get; set; } = new();
    }
}
