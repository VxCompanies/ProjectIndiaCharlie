﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Models;

namespace ProjectIndiaCharlie.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicController : ControllerBase
    {
        private readonly ProjectIndiaCharlieContext _context;

        public AcademicController(ProjectIndiaCharlieContext context) => _context = context;

        [HttpGet("Student/Subjects")]
        public async Task<ActionResult<IEnumerable<SubjectStudent>>> GetStudentSubjects(int studentId)
        {
            var subjects = await _context.SubjectStudents.Include(s => s.SubjectDetail.Subject)
                .Where(s => s.StudentId == studentId)
                .ToListAsync();

            return subjects is null ?
                NotFound() :
                Ok(subjects);
        }
    }
}