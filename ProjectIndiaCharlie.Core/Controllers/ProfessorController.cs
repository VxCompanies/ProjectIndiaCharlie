using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Models;
using System.Data;

namespace ProjectIndiaCharlie.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly ProjectIndiaCharlieContext _context;

        public ProfessorController(ProjectIndiaCharlieContext context) => _context = context;

        [HttpGet("Login")]
        public async Task<ActionResult<VProfessorDetail>> ProfessorLogin(int professorId, string password)
        {
            try
            {
                var passwordSalt = await _context.GetPasswordSalt(professorId);

                if (string.IsNullOrWhiteSpace(passwordSalt))
                    return NotFound();

                var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

                var professor = await _context.ProfessorLogin(professorId, passwordHash);

                return professor is null ?
                    NotFound("Wrong professor ID or password.") :
                    Ok(professor);
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message);
            }
        }

        [HttpPost("Registration")]
        public async Task<ActionResult<NewPerson>> RegisterProfessor(NewPerson newProfessor)
        {
            try
            {
                if (newProfessor.PersonId > 0)
                {
                    var flag = await _context.VProfessorDetails
                        .FindAsync(newProfessor.PersonId);

                    if (flag is not null)
                        return Conflict("Professor already registered.");
                }

                await _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
                if (newProfessor.PersonId == 0)
                {
                    await _context.PersonRegistration(newProfessor);

                    newProfessor.PersonId = (await _context.VPeopleDetails
                        .FirstAsync(p => p.DocNo == newProfessor.DocNo)).PersonId;
                }
                await _context.ProfessorRegistration(newProfessor);
                var newPassword = PasswordHelpers.GenRandomPassword();
                await _context.PasswordUpsert(newProfessor.PersonId, newPassword);
                newProfessor.Password = newPassword.Password;

                await _context.Database.CommitTransactionAsync();
                return CreatedAtAction("RegisterProfessor", newProfessor);
            }
            catch (Exception e)
            {
                await _context.Database.RollbackTransactionAsync();
                return Problem(detail: e.Message);
            }
        }

        // TODO: Waiting for Nikita
        [HttpGet("SubjectSections")]
        public async Task<ActionResult<IEnumerable<VSubjectSectionDetail>>> GetSubjectSections()
        {
            try
            {
                var subjects = _context.VSubjectSectionDetails;

                return subjects is null ?
                    NotFound() :
                    Ok(await subjects.ToListAsync());
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message);
            }
        }

        //// For professors
        //[HttpGet("Student/List")]
        //public async Task<ActionResult<IEnumerable<Student>>> GetStudents() =>
        //    (_context.Students is null) ?
        //    NotFound() :
        //    Ok(await _context.Students.Include(p => p.Person)
        //    .ToListAsync());
    }
}
