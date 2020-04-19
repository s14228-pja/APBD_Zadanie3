using System.Collections.Generic;
using System;
using System.Linq;
using Cwieczenie3.DAL;
using Cwieczenie3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cwieczenie3.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDbServices _dbService;

        public StudentController(IDbServices dbServices)
        {
            _dbService = dbServices;
        }

        [HttpGet]
        public IActionResult GetStudent(String nrIndeksu)
        {
            IEnumerable<Student> s = _dbService.GetStudent(nrIndeksu);
            if (s != null && s.Any())
            {
                return Ok(s);
            }
            return NotFound("Nie znaleziono studenta o numerze indeksu " + nrIndeksu);
        }
    }
}


