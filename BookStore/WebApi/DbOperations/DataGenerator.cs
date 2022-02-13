using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Entities;

namespace WebApi.DbOperations
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any())
                    return;

                context.Genres.AddRange(
                    new Genre
                    {
                        Name = "Personal Growth"
                    },
                    new Genre
                    {
                        Name = "Science Fiction"
                    },
                    new Genre
                    {
                        Name = "Romance"
                    }
                );

                context.Authors.AddRange(
                    new Author
                    {
                        FirstName = "Eric",
                        LastName = "Ries",
                        DateOfBirth = new DateTime(1978, 9, 22)
                    },
                    new Author
                    {
                        FirstName = "Charlotte",
                        LastName = "Gilman",
                        DateOfBirth = new DateTime(1860, 7, 3)
                    },
                    new Author
                    {
                        FirstName = "Frank",
                        LastName = "Herbert",
                        DateOfBirth = new DateTime(1920, 10, 8)
                    }
                );

                context.Books.AddRange(
                    new Book
                    {
                        Title = "Lean Startup",
                        GenreId = 1,
                        AuthorId = 1,
                        PageCount = 200,
                        PublishDate = new DateTime(2001, 6, 12)
                    },
                    new Book
                    {
                        Title = "Herland",
                        GenreId = 2,
                        AuthorId = 2,
                        PageCount = 250,
                        PublishDate = new DateTime(2010, 5, 23)
                    },
                    new Book
                    {
                        Title = "Dune",
                        GenreId = 2,
                        AuthorId = 3,
                        PageCount = 540,
                        PublishDate = new DateTime(2001, 12, 21)
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
