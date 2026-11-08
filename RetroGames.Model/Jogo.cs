namespace RetroGames.Model.Model
{
    public class Jogo
    {
        // Chave primária: co_seq_jogo
        public int Id { get; set; }

        // Nome do jogo: no_jogo
        public string Nome { get; set; } = string.Empty;

        // Descrição do jogo: ds_jogo
        public string? Descricao { get; set; }

        // Ano de lançamento: nu_ano_lancamento
        public int AnoLancamento { get; set; }

        // Preço do jogo: vl_preco
        public decimal Preco { get; set; }
        // Quantidade em estoque: qt_estoque
        public int Estoque { get; set; }
        // Indica se o jogo está ativo: st_ativo
        public bool Ativo { get; set; }

        // Data de cadastro: dt_cadastro
        public DateTime DataCadastro { get; set; }

        // Chave estrangeira da plataforma: co_plataforma
        public int IdPlataforma { get; set; }

        // Chave estrangeira da condição do jogo: co_condicao_jogo
        public int IdCondicaoJogo { get; set; }
    }
}