using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Queries.GetBookDetail;
using WebApi.DbOperations;
using Xunit;

namespace Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBookDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenInvalidInputsAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            GetBookDetailQuery command = new GetBookDetailQuery(_context, _mapper);
            command.BookId = 0;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Kitap bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeListed()
        {
            var book = _context.Books.FirstOrDefault();

            GetBookDetailQuery command = new GetBookDetailQuery(_context, _mapper);
            command.BookId = book.Id;

            var result = FluentActions.Invoking(() => command.Handle()).Invoke();

            result.Should().NotBeNull();
            result.Author.Should().Be(
                _context.Authors.SingleOrDefault(a => a.Id == book.AuthorId).FirstName + " " + 
                _context.Authors.SingleOrDefault(a => a.Id == book.AuthorId).LastName);
            result.Genre.Should().Be(
                _context.Genres.SingleOrDefault(g => g.Id == book.GenreId).Name);
            result.PageCount.Should().Be(book.PageCount);
            result.PublishDate.Should().Be(book.PublishDate.ToString());
            result.Title.Should().Be(book.Title);
        }
    }
}
