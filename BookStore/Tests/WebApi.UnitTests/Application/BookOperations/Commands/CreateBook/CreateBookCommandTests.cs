using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.CreateBook;
using WebApi.DbOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange (hazırlık)
            var book = new Book
            {
                Title = "Test_WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn",
                PublishDate = new DateTime(1990, 01, 10),
                PageCount = 100,
                AuthorId = 1,
                GenreId = 1
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            command.Model = new CreateBookModel { Title = book.Title };

            // act & assert (çalıştır ve doğrula)
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Kitap adı zaten mevcut");
        }

        [Fact]
        public void WhenInvalidAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange (hazırlık)
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            command.Model = new CreateBookModel
            {
                Title = "Test_WhenInvalidAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn",
                PublishDate = new DateTime(2000, 1, 1),
                PageCount = 100,
                AuthorId = 0,
                GenreId = _context.Genres.FirstOrDefault().Id
            };

            // act & assert (çalıştır ve doğrula)
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Yazar seçilmedi");
        }

        [Fact]
        public void WhenInvalidGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange (hazırlık)
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            command.Model = new CreateBookModel
            {
                Title = "Test_WhenInvalidGenreIdIsGiven_InvalidOperationException_ShouldBeReturn",
                PublishDate = new DateTime(2000, 1, 1),
                PageCount = 100,
                AuthorId = _context.Authors.FirstOrDefault().Id,
                GenreId = 0
            };

            // act & assert (çalıştır ve doğrula)
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("Tür seçilmedi");
        }

        [Fact]
        public void WhenValidInputIsGiven_Book_ShouldBeCreated()
        {
            // arrange
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            CreateBookModel model = new CreateBookModel
            {
                Title = "Test_WhenValidInputIsGiven_Book_ShouldBeCreated",
                PageCount = 1000,
                PublishDate = DateTime.Now.Date.AddYears(-10),
                AuthorId = 1,
                GenreId = 1
            };
            command.Model = model;

            // act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            // assert
            var book = _context.Books.SingleOrDefault(b => b.Title == model.Title);

            book.Should().NotBeNull();
            book.PageCount.Should().Be(model.PageCount);
            book.PublishDate.Should().Be(model.PublishDate);
            book.AuthorId.Should().Be(model.AuthorId);
            book.GenreId.Should().Be(model.GenreId);
        }
    }
}
