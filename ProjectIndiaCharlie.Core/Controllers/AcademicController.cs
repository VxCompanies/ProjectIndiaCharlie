using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        [HttpGet("Login")]
        public async Task<ActionResult<Person>> Login(int personId, string password)
        {
            await _context.Database.BeginTransactionAsync();
            try
            {
                var getPasswordSaltParameter = new SqlParameter("@personID", personId);
                if (await _context.Database.ExecuteSqlRawAsync("Academic.F_GetPasswordSalt", getPasswordSaltParameter) < 1)
                    return NotFound(); // User doesn't exists

                //var passwordHash = PasswordHelpers.GetPasswordHash(password, user.PersonPassword!.PasswordSalt);
                //var user = _context.VStudents.FromSqlRaw("", "");
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message); // DB problem
            }


            //if (user is null)
            //    return NotFound();


            //if (_context.People.FromSqlRaw("Person.F_Login", personId, passwordHash) is null)
            //    return NotFound();

            return /*(user.PersonPassword.PasswordHash == passwordHash) ?
                Ok(user) :*/
                NotFound();
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
