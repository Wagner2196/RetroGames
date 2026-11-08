namespace RetroGames.Model.Model
{
    public class Plataforma
    {
        // Chave primária: co_seq_plataforma
        public int Id { get; set; }

        // Nome da plataforma: no_plataforma
        public string Nome { get; set; } = string.Empty;

        // Fabricante da plataforma: no_fabricante
        public string Fabricante { get; set; } = string.Empty;

        // Ano de lançamento: nu_ano_lancamento
        public int AnoLancamento { get; set; }
    }
}