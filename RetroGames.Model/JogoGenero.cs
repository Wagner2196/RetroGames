namespace RetroGames.Model.Model
{
    public class JogoGenero
    {
        // Chave estrangeira do jogo: co_jogo
        public int IdJogo { get; set; }

        // Chave estrangeira do gênero: co_genero
        public int IdGenero { get; set; }
    }
}