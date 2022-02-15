using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.UpdateBook;
using Xunit;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData(0, "", 0, 0)]
        [InlineData(1, "", 0, 0)]
        [InlineData(0, "Lord Of The Rings", 0, 0)]
        [InlineData(0, "", 1, 0)]
        [InlineData(0, "", 0, 1)]
        [InlineData(1, "Lord Of The Rings", 0, 0)]
        [InlineData(1, "", 1, 0)]
        [InlineData(1, "", 0, 1)]
        [InlineData(0, "Lord Of The Rings", 1, 0)]
        [InlineData(0, "Lord Of The Rings", 0, 1)]
        [InlineData(0, "", 1, 1)]
        [InlineData(1, "Lord Of The Rings", 1, 0)]
        [InlineData(1, "Lord Of The Rings", 0, 1)]
        [InlineData(1, "", 1, 1)]
        [InlineData(0, "Lord Of The Rings", 1, 1)]
        [InlineData(1, "Lor", 1, 1)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int bookId, string title, int authorId, int genreId)
        {
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = bookId;
            command.Model = new UpdateBookModel
            {
                Title = title,
                AuthorId = authorId,
                GenreId = genreId
            };

            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = 1;
            command.Model = new UpdateBookModel
            {
                Title = "Test_WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError",
                AuthorId = 1,
                GenreId = 1
            };

            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}
