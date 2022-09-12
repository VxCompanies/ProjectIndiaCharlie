using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("Student/Login")]
        public async Task<ActionResult<Person>> StudentLogin(int personId, string password)
        {
            var passwordSalt = await _context.GetPasswordSalt(personId);

            if (string.IsNullOrWhiteSpace(passwordSalt))
                return NotFound();

            var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

            var student = await _context.StudentLogin(personId, passwordHash);

            return (student is null) ?
                NotFound() :
                Ok(student);
        }

        //[HttpGet("Student/Subjects")]
        //public async Task<ActionResult<IEnumerable<SubjectStudent>>> GetStudentSubjects(int studentId)
        //{
        //    var subjects = await _context.Student.Include(s => s.SubjectDetail.Subject)
        //        .Where(s => s.StudentId == studentId)
        //        .ToListAsync();

        //    return subjects is null ?
        //        NotFound() :
        //        Ok(subjects);
        //}
    }
}
