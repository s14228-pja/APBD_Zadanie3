using Cwieczenie3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwieczenie3.DAL
{
    public interface IDbServices
    {
        public IEnumerable<Student> GetStudents();
        public IEnumerable<Student> GetStudent(String nrIndeksu);
    }
}
