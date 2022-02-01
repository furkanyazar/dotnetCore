using FluentValidation;

namespace WebApi.BookOperations.UpdateBook
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(b => b.BookId).NotEmpty().GreaterThan(0);
            RuleFor(b => b.Model.GenreId).NotEmpty().GreaterThan(0);
            RuleFor(b => b.Model.Title).NotEmpty().MinimumLength(4);
        }
    }
}
