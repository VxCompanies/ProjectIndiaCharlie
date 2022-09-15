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
    //public async Task<ActionResult<Person>> StudentLogin(int personId, string password)
    //{
    //    try
    //    {
    //        var passwordSalt = await _context.GetPasswordSalt(personId);

    //        if (string.IsNullOrWhiteSpace(passwordSalt))
    //        {
    //            await _context.Database.RollbackTransactionAsync();
    //            return NotFound();
    //        }

    //        var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

    //        var student = await _context.StudentLogin(personId, passwordHash);

    //        return await student.FirstOrDefaultAsync() is null ?
    //            NotFound() :
    //            Ok(student);
    //    }
    //    catch (Exception e)
    //    {
    //        return Problem(detail: e.Message);
    //    }
    //}

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
    public async Task<ActionResult<string>> RegisterStudent(NewPerson newStudent)
    {
        var flag = new SqlParameter
        {
            ParameterName = "flag",
            SqlDbType = SqlDbType.NVarChar,
            Size = 6,
            Direction = ParameterDirection.Output
        };
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT {flag} = Academic.F_StudentValidation({newStudent})");
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }

        if (flag.Value is 1)
            return Conflict("Student already registered.");

        var studentRegistrationParams = new List<SqlParameter>
        {
            new("@DocNo", newStudent.DocNo),
            new("@FirstName", newStudent.FirstName),
            new("@MiddleName", newStudent.MiddleName),
            new("@FirstSurname", newStudent.FirstSurname),
            new("@SecondSurname", newStudent.SecondSurname),
            new("@Gender", newStudent.Gender),
            new("@BirthDate", newStudent.BirthDate),
            new("@Email", newStudent.Email)
        };
        try
        {
            await _context.Database.ExecuteSqlRawAsync("Person.SP_PersonRegistration", studentRegistrationParams);
        }
        catch (DbUpdateException e)
        {
            return Problem(detail: e.Message);
        }

        return CreatedAtAction("RegisterStudent", newStudent);
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
