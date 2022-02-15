using System;
using FluentValidation;

namespace WebApi.Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(b => b.Model.GenreId).NotEmpty().GreaterThan(0);
            RuleFor(b => b.Model.AuthorId).NotEmpty().GreaterThan(0);
            RuleFor(b => b.Model.PageCount).NotEmpty().GreaterThan(0);
            RuleFor(b => b.Model.PublishDate.Date).NotEmpty().LessThan(DateTime.Now.Date);
            RuleFor(b => b.Model.Title).NotEmpty().MinimumLength(4);
        }
    }
}
