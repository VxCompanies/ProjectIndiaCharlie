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

    [HttpPost("Registration")]
    public async Task<ActionResult<NewPerson>> RegisterStudent(NewPerson newStudent)
    {
        try
        {
            if (newStudent.PersonId > 0)
            {
                var flag = await _context.VStudentDetails
                    .FindAsync(newStudent.PersonId);

                if (flag is not null)
                    return Conflict("Student already registered.");
            }

            await _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
            if (newStudent.PersonId == 0)
            {
                await _context.PersonRegistration(newStudent);

                newStudent.PersonId = (await _context.VPeopleDetails
                    .FirstAsync(p => p.DocNo == newStudent.DocNo)).PersonId;
            }
            await _context.StudentRegistration(newStudent);
            var newPassword = PasswordHelpers.GenRandomPassword();
            await _context.PasswordUpsert(newStudent.PersonId, newPassword);
            newStudent.Password = newPassword.Password;

            await _context.Database.CommitTransactionAsync();
            return CreatedAtAction("RegisterStudent", newStudent);
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("Login")]
    public async Task<ActionResult<VStudentDetail>> StudentLogin(int personId, string password)
    {
        try
        {
            var passwordSalt = await _context.GetPasswordSalt(personId);

            if (string.IsNullOrWhiteSpace(passwordSalt))
                return NotFound();

            var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

            var student = await _context.StudentLogin(personId, passwordHash);

            return student is null ?
                NotFound("Wrong student ID or password.") :
                Ok(student);
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpPost("SubjectSelection")]
    public async Task<ActionResult<VStudentSubject>> SubjectSelection(int subjectId, int studentId)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var scheduleValidation = await _context.SubjectScheduleValidation(subjectId, studentId);
            if (!string.IsNullOrWhiteSpace(scheduleValidation))
            {
                await _context.Database.RollbackTransactionAsync();
                return Conflict($"Cannot select this subject because the schedule conflicts with '{scheduleValidation}'.");
            }

            await _context.SubjectSelection(subjectId, studentId);
            await _context.Database.CommitTransactionAsync();

            var subject = await _context.VStudentSubjects
                            .FirstOrDefaultAsync(s => s.SubjectDetailId == subjectId);
            return base.CreatedAtAction("SubjectSelection", subject);
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

    [HttpGet("Search")]
    public async Task<ActionResult<VStudentDetail>> GetStudent(string docNo)
    {
        try
        {
            var student = await _context.VStudentDetails
                .FirstOrDefaultAsync(s => s.DocNo == docNo);

            return (student is null) ?
                NotFound() :
                Ok(student);
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }
}
