
namespace RetroGames.Services.Errors;

// Representa uma exceção lançada quando uma regra de negócio
// do sistema é violada.
public class BusinessRuleException : Exception
{
    // Guarda o erro do catálogo que causou a exceção.
    // Assim, conseguimos acessar o código, a mensagem
    // e o status HTTP do erro.
    public BusinessError Erro { get; }

    // Construtor que recebe um erro já definido em BusinessErrors.
    public BusinessRuleException(BusinessError erro)
        // Envia a mensagem do erro para a classe Exception.
        : base(erro.Mensagem)
    {
        // Armazena o erro recebido na propriedade Erro.
        Erro = erro;
    }
}
