using System;
using FluentValidation;

namespace WebApi.BookOperations.CreateBook
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(b => b.Model.GenreId).GreaterThan(0);
            RuleFor(b => b.Model.PageCount).GreaterThan(0);
            RuleFor(b => b.Model.PublishDate.Date).NotEmpty().LessThan(DateTime.Now.Date);
            RuleFor(b => b.Model.Title).NotEmpty().MinimumLength(4);
        }
    }
}
