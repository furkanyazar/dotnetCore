using System;
using System.Linq;
using WebApi.DbOperations;

namespace WebApi.Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand
    {
        public int AuthorId { get; set; }

        private readonly IBookStoreDbContext _context;

        public DeleteAuthorCommand(IBookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(a => a.Id == AuthorId);

            if (author is null)
                throw new InvalidOperationException("Yazar bulunamadı");
            
            if (_context.Books.Any(b => b.AuthorId == AuthorId))
                throw new InvalidOperationException("Yazarın en az bir kitabı mevcut");

            _context.Authors.Remove(author);
            _context.SaveChanges();
        }
    }
}
