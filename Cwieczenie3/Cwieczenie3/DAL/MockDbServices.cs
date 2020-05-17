using Cwieczenie3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwieczenie3.DAL
{
    public class MockDbServices : IDbServices
    {
        private static IEnumerable<Student> _students;

        static MockDbServices ()
        {
            _students = new List<Student>
            {
                new Student{ FirstName="Jan", LastName="Kowalski" , IndexNumber = $"s{new Random().Next(1, 20000)}"},

                new Student{ FirstName="Anna", LastName="Malewska", IndexNumber = $"s{new Random().Next(1, 20000)}" },

                new Student{ FirstName="Andrzej ", LastName="Andrzejewicz", IndexNumber = $"s{new Random().Next(1, 20000)}" }
            };

        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
        public IEnumerable<Student> GetStudent( String nrIndeksu)
        {
            return _students;
        }

        public bool VerifyEnrolment(Student student)
        {
            throw new NotImplementedException();
        }

        public bool VerifyEnrolmentExists(Enrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public Enrollment EnrollStudent(Student request)
        {
            throw new NotImplementedException();
        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
            throw new NotImplementedException();
        }
    }
}
