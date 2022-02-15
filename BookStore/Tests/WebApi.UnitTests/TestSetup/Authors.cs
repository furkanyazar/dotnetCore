using System;
using WebApi.DbOperations;
using WebApi.Entities;

namespace TestSetup
{
    public static class Authors
    {
        public static void AddAuthors(this BookStoreDbContext context)
        {
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
        }
    }
}
