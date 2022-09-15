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
    public class AcademicController : ControllerBase
    {
        private readonly ProjectIndiaCharlieContext _context;

        public AcademicController(ProjectIndiaCharlieContext context) => _context = context;

        [HttpGet("Professor/Login")]
        public async Task<ActionResult<VProfessorDetail>> ProfessorLogin(int professorId, string password)
        {
            try
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
            catch (Exception e)
            {
                return Problem(detail: e.Message);
            }
        }

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

        [HttpPost("SubjectSelection")]
        public async Task<ActionResult<bool>> SubjectSelection(SubjectSchedule subject, int studentId)
        {
            var tran = _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
            var flag = new SqlParameter
            {
                ParameterName = "flag",
                SqlDbType = SqlDbType.NVarChar,
                Size = 6,
                Direction = ParameterDirection.Output
            };
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT {flag} = Academic.F_SubjectScheduleValidation({studentId}, {subject.WeekdayId}, {subject.StartTime}, {subject.EndTime})");

                if (flag.Value is not null)
                    return Problem(detail: $"There was a conflict with {flag.Value}.");

                await tran;
                await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC Academic.SP_SubjectSelection {subject.SubjectDetailId}, {studentId}");

                await _context.Database.CommitTransactionAsync();
                return Ok(true);
            }
            catch (Exception e)
            {
                await _context.Database.RollbackTransactionAsync();
                return Problem(detail: e.Message);
            }
        }

        [HttpPost("Professor/Registration")]
        public async Task<ActionResult<Professor>> RegisterProfessor(Professor professor)
        {
            var tran = _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

            if (await _context.Professors.FindAsync(professor.PersonId) != null)
                return Conflict();

            if (await _context.People.AnyAsync(p => p.DocNo == professor.Person.DocNo))
                professor.Person = new();

            var spParams = new List<SqlParameter>
            {
                new("@DocNo", professor.Person.DocNo),
                new("@FirstName", professor.Person.FirstName),
                new("@MiddleName", professor.Person.MiddleName),
                new("@FirstSurname", professor.Person.FirstSurname),
                new("@SecondSurname", professor.Person.SecondSurname),
                new("@Gender", professor.Person.Gender),
                new("@BirthDate", professor.Person.BirthDate),
                new("@Email", professor.Person.Email),
                new("@RolId", 2),
                new("@PasswordHash", professor.Person.PersonPassword!.PasswordHash),
                new("@PasswordSalt", professor.Person.PersonPassword!.PasswordSalt),
            };

            try
            {
                await tran;
                await _context.Database.ExecuteSqlRawAsync("Person.SP_RegisterPerson", spParams);
                await _context.Database.CommitTransactionAsync();
            }
            catch (DbUpdateException e)
            {
                await _context.Database.RollbackTransactionAsync();
                return Problem(detail: e.Message);
            }

            return CreatedAtAction("RegisterProfessor", professor);
        }

        // For professors
        [HttpGet("Student/List")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents() =>
            (_context.Students is null) ?
            NotFound() :
            Ok(await _context.Students.Include(p => p.Person)
            .ToListAsync());

        [HttpGet("Search")]
        public async Task<ActionResult<Person?>> GetPerson(string docNo)
        {
            var person = await _context.People
                .FirstOrDefaultAsync(p => p.DocNo == docNo);

            return person is null ?
                NotFound() :
                Ok(person);
        }
    }
}
