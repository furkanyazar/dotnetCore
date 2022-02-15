using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.DbOperations;

namespace WebApi.Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookDetailQuery
    {
        public int BookId { get; set; }

        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBookDetailQuery(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BookDetailViewModel Handle()
        {
            var book = _context.Books.Include(b => b.Genre).Include(b => b.Author).SingleOrDefault(b => b.Id == BookId);

            if (book is null)
                throw new InvalidOperationException("Kitap bulunamadı");

            return _mapper.Map<BookDetailViewModel>(book);
        }
    }

    public class BookDetailViewModel
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string PublishDate { get; set; }
    }
}
