using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Models;
using System.Data;

namespace ProjectIndiaCharlie.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly ProjectIndiaCharlieContext _context;

    public StudentController(ProjectIndiaCharlieContext context) => _context = context;

    [HttpGet("Login")]
    public async Task<ActionResult<Person>> StudentLogin(int personId, string password)
    {
        try
        {
            var passwordSalt = await _context.GetPasswordSalt(personId);

            if (string.IsNullOrWhiteSpace(passwordSalt))
            {
                await _context.Database.RollbackTransactionAsync();
                return NotFound();
            }

            var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

            var student = await _context.StudentLogin(personId, passwordHash);

            return await student.FirstOrDefaultAsync() is null ?
                NotFound() :
                Ok(student);
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("SelectedSubjects")]
    public async Task<ActionResult<IEnumerable<VStudentSubject>>> GetStudentSubject(int studentId)
    {
        try
        {
            var subjects = _context.VStudentSubjects
                    .Where(s => s.StudentId == studentId)
                    .ToListAsync();

            return (await subjects).Count < 1 ?
                NotFound() :
                Ok(await subjects);
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpPost("Registration")]
    public async Task<ActionResult<Student>> RegisterStudent(Student student)
    {
        var flag = new SqlParameter
        {
            ParameterName = "flag",
            SqlDbType = SqlDbType.NVarChar,
            Size = 6,
            Direction = ParameterDirection.Output
        };
        await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT {flag} = Academic.F_StudentValidation({student.PersonId})");

        if (flag.Value is 1)
            return Conflict("Student alredy registered.");

        var spParams = new List<SqlParameter>
        {
            new("@DocNo", student.Person.DocNo),
            new("@FirstName", student.Person.FirstName),
            new("@MiddleName", student.Person.MiddleName),
            new("@FirstSurname", student.Person.FirstSurname),
            new("@SecondSurname", student.Person.SecondSurname),
            new("@Gender", student.Person.Gender),
            new("@BirthDate", student.Person.BirthDate),
            new("@Email", student.Person.Email),
            new("@RolId", 1),
            new("@CareerId", student.Career.CareerId),
            new("@PasswordHash", student.Person.PersonPassword!.PasswordHash),
            new("@PasswordSalt", student.Person.PersonPassword!.PasswordSalt),
        };
        try
        {
            await _context.Database.ExecuteSqlRawAsync("Person.SP_RegisterPerson", spParams);
        }
        catch (DbUpdateException e)
        {
            return Problem(detail: e.Message);
        }

        return CreatedAtAction("RegisterStudent", student);
    }

    [HttpGet("Search")]
    public async Task<ActionResult<VStudentDetail>> GetStudent(string docNo)
    {
        var student = await _context.VStudentDetails
            .FirstOrDefaultAsync(s => s.DocNo == docNo);

        return (student is null) ?
            NotFound() :
            Ok(student);
    }
}
