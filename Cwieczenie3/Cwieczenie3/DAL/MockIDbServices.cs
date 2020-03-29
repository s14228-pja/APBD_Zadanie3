using Cwieczenie3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwieczenie3.DAL
{
    public class MockIDbService : IDbServices
    {
        private static IEnumerable<Student> _students;

        static MockIDbService ()
        {
            _students = new List<Student>
            {
                new Student{IdStudent = 1, FirstName="Jan", LastName="Kowalski" , IndexNumber = $"s{new Random().Next(1, 20000)}"},

                new Student{IdStudent = 2, FirstName="Anna", LastName="Malewska", IndexNumber = $"s{new Random().Next(1, 20000)}" },

                new Student{IdStudent = 3, FirstName="Andrzej", LastName="Andrzejewicz", IndexNumber = $"s{new Random().Next(1, 20000)}" }
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
