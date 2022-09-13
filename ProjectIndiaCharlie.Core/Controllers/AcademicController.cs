using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("Student/Login")]
        public async Task<ActionResult<Person>> StudentLogin(int personId, string password)
        {
            var passwordSalt = await _context.GetPasswordSalt(personId);

            if (string.IsNullOrWhiteSpace(passwordSalt))
                return NotFound();

            var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

            var student = await _context.StudentLogin(personId, passwordHash);

            return (await student.FirstOrDefaultAsync() is null) ?
                NotFound() :
                Ok(student);
        }
        
        [HttpGet("Professor/Login")]
        public async Task<ActionResult<Person>> ProfessorLogin(int professorId, string password)
        {
            var passwordSalt = await _context.GetPasswordSalt(professorId);

            if (string.IsNullOrWhiteSpace(passwordSalt))
                return NotFound();

            var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

            var professor = await _context.ProfessorLogin(professorId, passwordHash);

            return (await professor.FirstOrDefaultAsync() is null) ?
                NotFound() :
                Ok(professor);
        }

        [HttpGet("Student/Subjects")]
        public async Task<ActionResult<IEnumerable<StudentSubject>>> GetStudentSubjects(int studentId)
        {
            var subjects = _context.VStudentSubjects
                .Where(s => s.StudentId == studentId);

            return subjects is null ?
                NotFound() :
                Ok(await subjects.ToListAsync());
        }
    }
}
