using System;
using System.Linq;
using WebApi.DbOperations;

namespace WebApi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand
    {
        private readonly BookStoreDbContext _context;

        public int AuthorId { get; set; }
        public UpdateAuthorModel Model { get; set; }

        public UpdateAuthorCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(a => a.Id == AuthorId);

            if (author is null)
                throw new InvalidOperationException("Yazar bulunamadı");
            
            if (_context.Authors.Any(a => a.FirstName.ToLower() == author.FirstName.ToLower() && a.LastName.ToLower() == author.LastName.ToLower() && a.DateOfBirth == author.DateOfBirth && a.Id != AuthorId))
                throw new InvalidOperationException("Yazar zaten mevcut");
            
            author.FirstName = string.IsNullOrEmpty(Model.FirstName.Trim()) ? author.FirstName : Model.FirstName;
            author.LastName = string.IsNullOrEmpty(Model.LastName.Trim()) ? author.LastName : Model.LastName;
            author.DateOfBirth = Model.DateOfBirth;

            _context.SaveChanges();
        }
    }

    public class UpdateAuthorModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
