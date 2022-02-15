using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.DeleteAuthor;
using WebApi.DbOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidInputsAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = 0;

            FluentActions.Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Yazar bulunamadı");
        }

        [Fact]
        public void WhenAuthorHasBook_InvalidOperationException_ShouldBeReturn()
        {
            int authorId = _context.Books.FirstOrDefault().AuthorId;

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = authorId;

            FluentActions.Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Yazarın en az bir kitabı mevcut");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeDeleted()
        {
            var author = new Author
            {
                FirstName = "Test_FN_WhenValidInputsAreGiven_Book_ShouldBeDeleted",
                LastName = "Test_LN_WhenValidInputsAreGiven_Book_ShouldBeDeleted",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            int authorId = _context.Authors.SingleOrDefault(a => a.FirstName == author.FirstName).Id;

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = authorId;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var deletedAuthor = _context.Authors.SingleOrDefault(a => a.Id == authorId);

            deletedAuthor.Should().BeNull();
        }
    }
}
