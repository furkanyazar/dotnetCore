using System;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.UpdateAuthor;
using Xunit;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData(0, "", "")]
        [InlineData(0, "Test_First_Name", "")]
        [InlineData(0, "", "Test_Last_Name")]
        [InlineData(0, "T", "T")]
        [InlineData(1, "", "")]
        [InlineData(1, "Test_First_Name", "")]
        [InlineData(1, "", "Test_Last_Name")]
        [InlineData(1, "T", "T")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int authorId, string firstName, string lastName)
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.AuthorId = authorId;
            command.Model = new UpdateAuthorModel
            {
                FirstName = firstName,
                LastName = lastName,
            };

            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldBeReturnError()
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.Model = new UpdateAuthorModel
            {
                DateOfBirth = DateTime.Now.Date
            };

            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.AuthorId = 1;
            command.Model = new UpdateAuthorModel
            {
                FirstName = "Test_FN_WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError",
                LastName = "Test_LN_WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}
