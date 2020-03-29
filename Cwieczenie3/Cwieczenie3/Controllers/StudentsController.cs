using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cwieczenie3.DAL;
using Cwieczenie3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cwieczenie3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDbServices _dbService;

        public StudentsController (IDbServices dbServices)
        {
            _dbService = dbServices;
        }

        [HttpGet]
        public IActionResult GetStudent(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            } else if (id == 2)
            {
                return Ok("Malewski");
            }
            return NotFound("Nie znaleziono studenta");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            // add to database
            // genrating index number

            student.IndexNumber = $"s{new Random().Next(1, 20000)}";

            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student student)
        {
            // add to database
            // genrating index number
            

            return Ok("Aktualizacja danych zakonczona sukcesem");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            // add to database
            // genrating index number


            return Ok("Usuwanie ukończone");
        }
    }
}