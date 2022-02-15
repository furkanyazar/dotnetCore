using System;
using System.Linq;
using AutoMapper;
using WebApi.DbOperations;
using WebApi.Entities;

namespace WebApi.Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommand
    {
        public CreateBookModel Model { get; set; }

        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateBookCommand(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Handle()
        {
            var book = _context.Books.SingleOrDefault(b => b.Title == Model.Title);

            if (book is not null)
                throw new InvalidOperationException("Kitap adı zaten mevcut");

            if (!_context.Authors.Any(a => a.Id == Model.AuthorId))
                throw new InvalidOperationException("Yazar seçilmedi");

            if (!_context.Genres.Any(g => g.Id == Model.GenreId))
                throw new InvalidOperationException("Tür seçilmedi");

            book = _mapper.Map<Book>(Model);

            _context.Books.Add(book);
            _context.SaveChanges();
        }
    }

    public class CreateBookModel
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
