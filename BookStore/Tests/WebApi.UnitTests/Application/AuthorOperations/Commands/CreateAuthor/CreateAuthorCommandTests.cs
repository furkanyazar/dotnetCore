using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.CreateAuthor;
using WebApi.DbOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var author = new Author
            {
                FirstName = "Test_FN_WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn",
                LastName = "Test_LN_WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn",
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
            command.Model = new CreateAuthorModel
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
            };

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Yazar zaten mevcut");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeCreated()
        {
            CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
            CreateAuthorModel model = new CreateAuthorModel
            {
                FirstName = "Test_FN_WhenValidInputIsGiven_Author_ShouldBeCreated",
                LastName = "Test_LN_WhenValidInputIsGiven_Author_ShouldBeCreated",
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var author = _context.Authors.SingleOrDefault(a => a.FirstName == model.FirstName);

            author.Should().NotBeNull();
        }
    }
}
