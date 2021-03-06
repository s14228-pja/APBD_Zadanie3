﻿using Cwieczenie3.DTOs;
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
        public Boolean VerifyEnrolment(Student student);
        public Boolean VerifyEnrolmentExists(Enrollment enrollment);
        public Enrollment EnrollStudent(Student request);
        public Enrollment PromoteStudents(int semester, string studies);
        public String Login(String indexNumber);
    }
}
