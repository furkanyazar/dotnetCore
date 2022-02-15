using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.DeleteGenre;
using WebApi.DbOperations;
using WebApi.Entities;
using Xunit;

namespace Application.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = 0;

            FluentActions.Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Kitap türü bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeDeleted()
        {
            var genre = new Genre
            {
                Name = "Test_WhenValidInputsAreGiven_Book_ShouldBeDeleted"
            };

            _context.Genres.Add(genre);
            _context.SaveChanges();

            int genreId = _context.Genres.SingleOrDefault(g => g.Name == genre.Name).Id;

            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = genreId;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var deletedGenre = _context.Genres.SingleOrDefault(g => g.Id == genreId);

            deletedGenre.Should().BeNull();
        }
    }
}
