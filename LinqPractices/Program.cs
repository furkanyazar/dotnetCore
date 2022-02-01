using System.Linq;
using LinqPractices.DbOperations;
using LinqPractices.Entities;

namespace LinqPractices
{
    class Program
    {
        static void Main(string[] args)
        {
            DataGenerator.Initialize();
            LinqDbContext _context = new LinqDbContext();
            var students = _context.Students.ToList<Student>();

            // Find()
            System.Console.WriteLine("**** Find ****");
            var student = _context.Students.Where(s => s.StudentId == 1).FirstOrDefault();
            student = _context.Students.Find(2);
            System.Console.WriteLine(student.Name);

            // FirstOrDefault()
            System.Console.WriteLine("\n**** FirstOrDefault ****");
            student = _context.Students.Where(s => s.Surname == "Arda").FirstOrDefault();
            System.Console.WriteLine(student.Name);
            student = _context.Students.FirstOrDefault(s => s.Surname == "Arda");
            System.Console.WriteLine(student.Name);

            // SingleOrDefault()
            System.Console.WriteLine("\n**** SingleOrDefault ****");
            student = _context.Students.SingleOrDefault(s => s.Name == "Deniz");
            System.Console.WriteLine(student.Surname);

            // ToList()
            System.Console.WriteLine("\n**** ToList ****");
            var studentList = _context.Students.Where(s => s.ClassId == 2).ToList();
            System.Console.WriteLine(studentList.Count);

            // OrderBy()
            System.Console.WriteLine("\n**** OrderBy ****");
            studentList = _context.Students.OrderBy(s => s.StudentId).ToList();
            foreach (var item in studentList)
            {
                System.Console.WriteLine(item.StudentId + " - " + item.Name + " " + item.Surname);
            }

            // OrderByDescending()
            System.Console.WriteLine("\n**** OrderByDescending ****");
            studentList = _context.Students.OrderByDescending(s => s.StudentId).ToList();
            foreach (var item in studentList)
            {
                System.Console.WriteLine(item.StudentId + " - " + item.Name + " " + item.Surname);
            }

            // Anonymous Object Result
            System.Console.WriteLine("\n**** Anonymous Object Result ****");
            var anonymousObject = _context.Students.Where(s => s.ClassId == 2).Select(s => new
            {
                Id = s.StudentId,
                FullName = s.Name + " " + s.Surname
            });
            foreach (var item in anonymousObject)
            {
                System.Console.WriteLine(item.Id + " - " + item.FullName);
            }
        }
    }
}
