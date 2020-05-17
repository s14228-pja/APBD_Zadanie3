using Cwieczenie3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cwieczenie3.DAL
{
    public class DbServices : IDbServices
    {


        public IEnumerable<Student> GetStudents()
        {
            List<Student> _students = new List<Student>();
            using (var con = new SqlConnection("Data Source = db-mssql; Initial Catalog = s14228; Integrated Security = True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from [dbo].[Student]";
                con.Open();

                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    _students.Add(st);
                }


            }
            return _students;
        }

        public IEnumerable<Student> GetStudent(String nrIndeksu)
        {
            List<Student> _students = new List<Student>();
            using (var con = new SqlConnection("Data Source = db-mssql; Initial Catalog = s14228; Integrated Security = True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select Student.FirstName, Student.LastName, Student.IndexNumber, Student.BirthDate, Enrollment.Semester, Studies.Name from Studies join Enrollment on Studies.IdStudy = Enrollment.IdStudy join Student on Enrollment.IdEnrollment = Student.IdEnrollment where Student.IndexNumber = @nrIndeksu";
                com.Parameters.AddWithValue("nrIndeksu", nrIndeksu);
                con.Open();

                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    st.StudiesName = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();
                    _students.Add(st);

                }


            }
            return _students;
        }

        public bool VerifyEnrolment(Student student)
        {
            using (var con = new SqlConnection("Data Source = db-mssql; Initial Catalog = s14228; Integrated Security = True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                //1. Czy studia istnieja?
                com.CommandText = "select * from studies where studies.name=@name ";
                com.Parameters.AddWithValue("name", student.StudiesName);
                con.Open();
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return false;
                }
                return true;
            }
        }

        public bool VerifyEnrolmentExists(Enrollment enrollment)
        {
            using (var con = new SqlConnection("Data Source = db-mssql; Initial Catalog = s14228; Integrated Security = True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                //1. Czy studia istnieja?
                com.CommandText = "select * from studies join Enrollment on studies.idStudy = Enrollment.idStudy where studies.name=@name and enrollment.semester =@semester";
                com.Parameters.AddWithValue("name", enrollment.Studies);
                com.Parameters.AddWithValue("semester", enrollment.Semester);
                con.Open();
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return false;
                }
                return true;
            }
        }

        public void EnrollStudent(Student request)
        {
            
                throw new NotImplementedException();
            

        }

        public void PromoteStudents(int semester, string studies)
        {
            throw new NotImplementedException();
        }
    }
}
