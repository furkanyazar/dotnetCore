using FluentValidation;

namespace WebApi.Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(g => g.GenreId).NotEmpty().GreaterThan(0);
            RuleFor(g => g.Model.Name).MinimumLength(4).When(g => g.Model.Name.Trim() != string.Empty);
        }
    }
}
