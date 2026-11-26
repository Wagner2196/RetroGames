
using FluentValidation;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Validators.Jogo
{
    // Valida os parâmetros utilizados na listagem de jogos.
    public class ListarJogoRequestValidator
        : AbstractValidator<ListarJogoRequestApiModel>
    {
        public ListarJogoRequestValidator()
        {
            // A página precisa ser maior que zero.
            RuleFor(modelo => modelo.Pagina)
                .GreaterThan(0)
                .WithMessage("A página deve ser maior que zero.");

            // O tamanho da página deve estar entre 1 e 50.
            RuleFor(modelo => modelo.TamanhoPagina)
                .InclusiveBetween(1, 50)
                .WithMessage("O tamanho da página deve estar entre 1 e 50.");
        }
    }
}
