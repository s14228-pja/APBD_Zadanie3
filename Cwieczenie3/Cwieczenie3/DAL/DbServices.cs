using Cwieczenie3.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
                    st.Semester = int.Parse(dr["Semester"].ToString());
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

        public Enrollment EnrollStudent(Student request)
        {  
            var en = new Enrollment();
            
            using (var con = new SqlConnection("Data Source = db-mssql; Initial Catalog = s14228; Integrated Security = True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();

                try
                {
                    //1. Czy studia istnieja?
                    com.CommandText = "select IdStudies from studies where name=@name";
                    com.Parameters.AddWithValue("name", request.StudiesName);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        tran.Rollback();
                        return null;
                    }
                    int idstudies = (int)dr["IdStudies"];

                    com.CommandText = "select IdEnrollment from Enrollment where idStudy=@idstudies and semester = 1";
                    com.Parameters.AddWithValue("idstudies", idstudies);

                    var dr2 = com.ExecuteReader();
                    int idEnrollment;
                    if (!dr2.Read())
                    {
                        DateTime dateTimeVariable = DateTime.Now;
                        com.CommandText = "INSERT INTO IdEnrollment(Semester, IdStudy, StartDate) VALUES(@Semester, @IdStudies, @Today)";
                        com.Parameters.AddWithValue("Semester", 1);
                        com.Parameters.AddWithValue("IdStudies", idstudies);
                        com.Parameters.AddWithValue("Today", dateTimeVariable);
                        com.ExecuteNonQuery();

                        com.CommandText = "select IdEnrollment from Enrollment where idStudy=@idstudies and semester = '1'";
                        com.Parameters.AddWithValue("idstudies", idstudies);

                        var dr3 = com.ExecuteReader();
                        idEnrollment = (int)dr3["IdEnrollment"];

                    } else
                    {
                         idEnrollment = (int)dr2["IdEnrollment"];
                    }

                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES(@Index, @Fname, @Lname, @BirthDate, @IdEnrollment)";
                    com.Parameters.AddWithValue("index", request.IndexNumber);
                    com.Parameters.AddWithValue("Fname", request.FirstName);
                    com.Parameters.AddWithValue("Lname", request.LastName);
                    com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                    com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                    com.ExecuteNonQuery();

                    tran.Commit();

                    en.Semester = 1;
                    en.Studies = request.StudiesName;
                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                    return null;
                }
            }
            return en;

        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
            var en = new Enrollment();
            en.Studies = studies;
            en.Semester = semester + 1;
            using (var con = new SqlConnection("Data Source = db-mssql; Initial Catalog = s14228; Integrated Security = True"))
            
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("PromoteStudents", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Semester", semester));
                    cmd.Parameters.Add(new SqlParameter("@StudiyName", studies));
                    cmd.ExecuteNonQuery();
                } catch (Exception exc)
                {
                    return null;
                }
            }
            return en;
        }

        public string Login(string indexNumber)
        {
            String password ="";
            using (var con = new SqlConnection("Data Source = db-mssql; Initial Catalog = s14228; Integrated Security = True"))
            using (var com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "select Password from Student where IndexNumber = @indexNumber ";
                com.Parameters.AddWithValue("indexNumber", indexNumber);
                con.Open();

                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    password = dr["Password"].ToString();
                }


            }
            return password;
        }
    }
}
