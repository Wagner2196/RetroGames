
namespace RetroGames.Repository
{
    // Exceção específica do Repository para indicar
    // que um registro possui um valor duplicado.
    public class RegistroDuplicadoException : Exception
    {
        // Construtor que recebe uma mensagem explicando o erro.
        public RegistroDuplicadoException(string mensagem)
            : base(mensagem)
        {
        }
    }
}
