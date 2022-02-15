using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.UpdateBook;
using WebApi.DbOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public UpdateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidInputsAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = 0;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Kitap bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            var book = new Book
            {
                Title = "Test_WhenValidInputsAreGiven_Book_ShouldBeUpdated",
                PublishDate = new DateTime(2000, 1, 1),
                PageCount = 100,
                AuthorId = _context.Authors.FirstOrDefault().Id,
                GenreId = _context.Genres.FirstOrDefault().Id
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            int bookId = _context.Books.SingleOrDefault(b => b.Title == book.Title).Id;

            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = bookId;

            UpdateBookModel model = new UpdateBookModel
            {
                Title = "Updated_WhenValidInputsAreGiven_Book_ShouldBeUpdated",
                AuthorId = _context.Authors.FirstOrDefault().Id,
                GenreId = _context.Genres.FirstOrDefault().Id
            };
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var updatedBook = _context.Books.SingleOrDefault(b => b.Id == bookId);

            updatedBook.Should().NotBeNull();
            updatedBook.Title.Should().Be(model.Title);
            updatedBook.AuthorId.Should().Be(model.AuthorId);
            updatedBook.GenreId.Should().Be(model.GenreId);
        }
    }
}
