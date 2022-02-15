using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Queries.GetAuthorDetail;
using WebApi.DbOperations;
using Xunit;

namespace Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenInvalidInputsAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            GetAuthorDetailQuery command = new GetAuthorDetailQuery(_context, _mapper);
            command.AuthorId = 0;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Yazar bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeListed()
        {
            var author = _context.Authors.FirstOrDefault();

            GetAuthorDetailQuery command = new GetAuthorDetailQuery(_context, _mapper);
            command.AuthorId = author.Id;

            var result = FluentActions.Invoking(() => command.Handle()).Invoke();

            result.Should().NotBeNull();
            result.Id.Should().Be(author.Id);
            result.Name.Should().Be(author.FirstName + " " + author.LastName);
            result.DateOfBirth.Should().Be(author.DateOfBirth);
        }
    }
}
