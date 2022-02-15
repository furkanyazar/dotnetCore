using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.UpdateAuthor;
using WebApi.DbOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public UpdateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidInputsAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = 0;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Yazar bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            var author = new Author
            {
                FirstName = "Test_FN_WhenValidInputsAreGiven_Book_ShouldBeUpdated",
                LastName = "Test_LN_WhenValidInputsAreGiven_Book_ShouldBeUpdated",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            int authorId = _context.Authors.SingleOrDefault(a => a.FirstName == author.FirstName).Id;

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = authorId;

            UpdateAuthorModel model = new UpdateAuthorModel
            {
                FirstName = "Updated_FN_WhenValidInputsAreGiven_Book_ShouldBeUpdated",
                LastName = "Updated_LN_WhenValidInputsAreGiven_Book_ShouldBeUpdated",
                DateOfBirth = author.DateOfBirth.AddYears(1)
            };
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var updatedAuthor = _context.Authors.SingleOrDefault(a => a.Id == authorId);

            updatedAuthor.Should().NotBeNull();
            updatedAuthor.FirstName.Should().Be(model.FirstName);
            updatedAuthor.LastName.Should().Be(model.LastName);
            updatedAuthor.DateOfBirth.Should().Be(model.DateOfBirth);
        }
    }
}
