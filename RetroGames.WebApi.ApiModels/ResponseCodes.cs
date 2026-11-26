
namespace RetroGames.WebApi.ApiModels
{
    public static class ResponseCodes
    {
        // Código 0 indica que a operação foi realizada com sucesso.
        public const int SUCCESS = 0;

        // Código 1 indica que ocorreu um erro inesperado no servidor.
        public const int ERROR = 1;

        // Código 2 indica que o modelo esperado não foi enviado
        // ou chegou nulo na requisição.
        public const int MODEL_NULL = 2;

        // Código 3 indica que os dados enviados chegaram,
        // mas apresentaram erros de validação.
        public const int MODEL_STATE_INVALIDO = 3;
    }
}
