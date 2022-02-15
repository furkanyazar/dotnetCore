using System;
using FluentValidation;

namespace WebApi.Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(a => a.Model.FirstName).NotEmpty().MinimumLength(2);
            RuleFor(a => a.Model.LastName).NotEmpty().MinimumLength(2);
            RuleFor(a => a.Model.DateOfBirth).NotEmpty().LessThan(DateTime.Now.Date);
        }
    }
}
