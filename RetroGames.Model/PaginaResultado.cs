
namespace RetroGames.Model.Model
{
    // Representa o resultado de uma consulta paginada.
    //
    // O tipo T permite reutilizar essa classe para diferentes modelos.
    // Por exemplo: PaginaResultado<Jogo>.
    public class PaginaResultado<T>
    {
        // Contém os registros retornados na página atual.
        public List<T> Itens { get; set; } = new();

        // Informa quantos registros existem no total,
        // considerando os filtros aplicados.
        //
        // Esse número não representa apenas os itens da página.
        public int Total { get; set; }
    }
}
