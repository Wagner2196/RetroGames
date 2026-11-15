
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
    // Erro 7: plataforma não encontrada.
    public static readonly BusinessError PlataformaNaoEncontrada =
        new(7, "Plataforma não encontrada.", HttpStatusCode.NotFound);

    // Erro 8: nome de plataforma duplicado.
    public static readonly BusinessError PlataformaNomeDuplicado =
        new(8, "Já existe uma plataforma com este nome.", HttpStatusCode.Conflict);

    // Erro 9: plataforma possui jogos vinculados.
    public static readonly BusinessError PlataformaPossuiJogos =
        new(9, "Não é possível excluir: existem jogos desta plataforma.", HttpStatusCode.Conflict);

    // Erro 10: jogo não encontrado.
    public static readonly BusinessError JogoNaoEncontrado =
        new(10, "Jogo não encontrado.", HttpStatusCode.NotFound);

    // Erro 12: condição informada não existe.
    public static readonly BusinessError CondicaoNaoEncontrada =
        new(12, "A condição informada não existe.", HttpStatusCode.NotFound);

    // Erro 13: um ou mais gêneros informados não existem.
    public static readonly BusinessError GeneroNaoEncontrado =
        new(13, "Um ou mais gêneros informados não existem.", HttpStatusCode.NotFound);
}
