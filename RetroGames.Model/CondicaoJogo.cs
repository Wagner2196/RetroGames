namespace RetroGames.Model.Model
{
    public class CondicaoJogo
    {
        // Chave primária: co_condicao_jogo
        public int Id { get; set; }

        // Descrição da condição: ds_condicao_jogo
        public string Descricao { get; set; } = string.Empty;
    }
}