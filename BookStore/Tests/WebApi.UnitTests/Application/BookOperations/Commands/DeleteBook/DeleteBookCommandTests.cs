using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.DeleteBook;
using WebApi.DbOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = 0;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Kitap bulunamadı");
        }

        [Fact]
        public void WhenValidBookIdIsGiven_Book_ShouldBeDeleted()
        {
            var book = new Book
            {
                Title = "Test_WhenValidBookIdIsGiven_Book_ShouldBeDeleted",
                PublishDate = new DateTime(2000, 1, 1),
                PageCount = 100,
                AuthorId = 1,
                GenreId = 1
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            int bookId = _context.Books.SingleOrDefault(b => b.Title == book.Title).Id;

            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = bookId;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var deletedBook = _context.Books.SingleOrDefault(b => b.Id == bookId);

            deletedBook.Should().BeNull();
        }
    }
}
