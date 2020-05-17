﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cwieczenie3.DAL;
using Cwieczenie3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cwieczenie3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbServices _dbService;

        public EnrollmentsController(IDbServices dbServices)
        {
            _dbService = dbServices;
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {


        if (student.IndexNumber == null || student.IndexNumber.Trim().Length == 0 
                || student.FirstName == null || student.FirstName.Trim().Length == 0
                || student.LastName == null || student.LastName.Trim().Length == 0 
                || student.BirthDate == null
                || student.StudiesName == null || student.StudiesName.Trim().Length == 0
                )
            {
                return BadRequest();
            }
        if (!_dbService.VerifyEnrolment(student))
            {
                return BadRequest();
            }
            Enrollment enrollment = _dbService.EnrollStudent(student);
            if (enrollment == null)
            {
                return BadRequest();
            }
                return Ok(enrollment);
        }

        [Route("api/enrollments/promotions")]
        [HttpPost]
        public IActionResult PromoteStudent(Enrollment enrollment)
        {

            if (!_dbService.VerifyEnrolmentExists(enrollment))
            {
                return BadRequest();
            }
            Enrollment enrollment2 = new Enrollment();
            return Ok(enrollment2);
        }
    }
}