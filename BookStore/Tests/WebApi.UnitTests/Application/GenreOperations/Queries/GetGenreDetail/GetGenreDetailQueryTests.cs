using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.DbOperations;
using Xunit;

namespace Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetGenreDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenInvalidInputsAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            GetGenreDetailQuery command = new GetGenreDetailQuery(_context, _mapper);
            command.GenreId = 0;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Kitap türü bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeListed()
        {
            var genre = _context.Genres.FirstOrDefault();

            GetGenreDetailQuery command = new GetGenreDetailQuery(_context, _mapper);
            command.GenreId = genre.Id;

            var result = FluentActions.Invoking(() => command.Handle()).Invoke();

            result.Should().NotBeNull();
            result.Name.Should().Be(genre.Name);
        }
    }
}
