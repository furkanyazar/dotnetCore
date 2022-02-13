using System;
using System.Linq;
using WebApi.DbOperations;

namespace WebApi.Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand
    {
        public int AuthorId { get; set; }

        private readonly BookStoreDbContext _context;

        public DeleteAuthorCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(a => a.Id == AuthorId);

            var books = _context.Books.Where(b => b.AuthorId == AuthorId);

            if (author is null)
                throw new InvalidOperationException("Yazar bulunamadı");
            
            if (books is not null)
                throw new InvalidOperationException("Yazarın en az bir kitabı mevcut");

            _context.Authors.Remove(author);
            _context.SaveChanges();
        }
    }
}
