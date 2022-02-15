using System;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.CreateBook;
using Xunit;

namespace Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("", 0, 0, 0)]
        [InlineData("", 0, 0, 1)]
        [InlineData("Lord Of The Rings", 0, 0, 0)]
        [InlineData("", 1, 0, 0)]
        [InlineData("", 0, 1, 0)]
        [InlineData("Lord Of The Rings", 0, 0, 1)]
        [InlineData("", 1, 0, 1)]
        [InlineData("", 0, 1, 1)]
        [InlineData("Lord Of The Rings", 1, 0, 0)]
        [InlineData("Lord Of The Rings", 0, 1, 0)]
        [InlineData("", 1, 1, 0)]
        [InlineData("Lord Of The Rings", 1, 0, 1)]
        [InlineData("Lord Of The Rings", 0, 1, 1)]
        [InlineData("", 1, 1, 1)]
        [InlineData("Lord Of The Rings", 1, 1, 0)]
        [InlineData("Lor", 1, 1, 1)]
        public void WhenInvalidInputsIsGiven_Validator_ShouldBeReturnErrors(string title, int pageCount, int genreId, int authorId)
        {
            // arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel
            {
                Title = title,
                PageCount = pageCount,
                PublishDate = DateTime.Now.Date.AddYears(-1),
                GenreId = genreId,
                AuthorId = authorId
            };

            // act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldBeReturnError()
        {
            // arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel
            {
                Title = "Lord Of The Rings",
                PageCount = 100,
                PublishDate = DateTime.Now.Date,
                GenreId = 1,
                AuthorId = 1
            };

            // act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputIsGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel
            {
                Title = "Test_WhenValidInputIsGiven_Validator_ShouldNotBeReturnError",
                PageCount = 100,
                PublishDate = DateTime.Now.Date.AddYears(-2),
                GenreId = 1,
                AuthorId = 1
            };

            // act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
