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
                    Console.WriteLine(st.ToString());
                    _students.Add(st);
                }


            }
            return _students;
        }
    }
}
