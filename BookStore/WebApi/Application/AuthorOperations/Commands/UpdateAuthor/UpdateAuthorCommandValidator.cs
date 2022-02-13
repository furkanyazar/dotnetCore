using System;
using FluentValidation;

namespace WebApi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(a => a.AuthorId).NotEmpty().GreaterThan(0);
            RuleFor(a => a.Model.FirstName).NotEmpty().MinimumLength(2);
            RuleFor(a => a.Model.LastName).NotEmpty().MinimumLength(2);
            RuleFor(a => a.Model.DateOfBirth).NotEmpty().LessThan(DateTime.Now);
        }
    }
}
