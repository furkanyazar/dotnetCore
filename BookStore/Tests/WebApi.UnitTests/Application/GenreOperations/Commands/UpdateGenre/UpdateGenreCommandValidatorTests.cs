using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.UpdateGenre;
using Xunit;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData(0, "")]
        [InlineData(0, "Test Genre")]
        [InlineData(1, "")]
        [InlineData(1, "Tes")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int genreId, string name)
        {
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            command.GenreId = genreId;
            command.Model = new UpdateGenreModel
            {
                Name = name,
            };

            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            command.GenreId = 1;
            command.Model = new UpdateGenreModel
            {
                Name = "Test_WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError",
            };

            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}
