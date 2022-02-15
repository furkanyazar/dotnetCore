using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.UpdateGenre;
using WebApi.DbOperations;
using WebApi.Entities;
using Xunit;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public UpdateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = 0;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Kitap türü bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            var genre = new Genre
            {
                Name = "Test_WhenValidInputsAreGiven_Book_ShouldBeUpdated"
            };

            _context.Genres.Add(genre);
            _context.SaveChanges();

            int genreId = _context.Genres.SingleOrDefault(g => g.Name == genre.Name).Id;

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = genreId;

            UpdateGenreModel model = new UpdateGenreModel
            {
                Name = "Updated_WhenValidInputsAreGiven_Book_ShouldBeUpdated",
                IsActive = false
            };
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var updatedGenre = _context.Genres.SingleOrDefault(g => g.Id == genreId);

            updatedGenre.Should().NotBeNull();
            updatedGenre.Name.Should().Be(model.Name);
            updatedGenre.IsActive.Should().Be(model.IsActive);
        }
    }
}
