
using System.Net;

namespace RetroGames.Services.Errors;

// Representa um erro de negócio com três informações:
// código, mensagem e status HTTP.
public sealed record BusinessError(
    int Codigo,
    string Mensagem,
    HttpStatusCode StatusHttp
);

// Guarda todos os erros de negócio do sistema em um único lugar.
// Assim, evitamos repetir códigos e mensagens em várias classes.
public static class BusinessErrors
{
    // Erro 100: acontece quando a plataforma procurada não existe.
    // HTTP 404 significa que o recurso não foi encontrado.
    public static readonly BusinessError PlataformaNaoEncontrada =
        new(100, "Plataforma não encontrada.", HttpStatusCode.NotFound);

    // Erro 101: acontece quando tentamos cadastrar uma plataforma
    // com um nome que já está sendo utilizado.
    // HTTP 409 significa conflito de informações.
    public static readonly BusinessError PlataformaNomeDuplicado =
        new(101, "Já existe uma plataforma com este nome.", HttpStatusCode.Conflict);

    // Erro 102: acontece quando tentamos excluir uma plataforma
    // que ainda possui jogos cadastrados.
    public static readonly BusinessError PlataformaPossuiJogos =
        new(102, "Não é possível excluir: existem jogos desta plataforma.", HttpStatusCode.Conflict);

    // Erro 103: acontece quando o jogo informado não existe.
    public static readonly BusinessError JogoNaoEncontrado =
        new(103, "Jogo não encontrado.", HttpStatusCode.NotFound);

    // Erro 104: acontece quando a condição de conservação
    // informada para um jogo não existe.
    public static readonly BusinessError CondicaoNaoEncontrada =
        new(104, "A condição informada não existe.", HttpStatusCode.NotFound);

    // Erro 105: acontece quando um ou mais gêneros informados
    // para um jogo não existem.
    public static readonly BusinessError GeneroNaoEncontrado =
        new(105, "Um ou mais gêneros informados não existem.", HttpStatusCode.NotFound);
}
