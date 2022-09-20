using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<VStudentDetail>> StudentLogin(int studentId, string password)
    {
        try
        {
            var passwordSalt = await _context.GetPasswordSalt(studentId);

            if (string.IsNullOrWhiteSpace(passwordSalt))
                return NotFound();

            var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

            var student = await _context.StudentLogin(studentId, passwordHash);

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
    public async Task<ActionResult<string>> SubjectSelection(int subjectDetailID, int studentId)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var scheduleValidation = await _context.SubjectScheduleValidation(subjectDetailID, studentId);
            if (!string.IsNullOrWhiteSpace(scheduleValidation))
            {
                await _context.Database.RollbackTransactionAsync();
                return Conflict($"Cannot select this subject because the schedule conflicts with '{scheduleValidation}'.");
            }

            if (!await _context.SubjectSelection(subjectDetailID, studentId))
                return Conflict($"Subject section is full.");

            await _context.Database.CommitTransactionAsync();

            var subject = await _context.VStudentSubjects
                            .FirstAsync(s => s.SubjectDetailId == subjectDetailID);
            return base.CreatedAtAction("SubjectSelection", $"Successfully selected {subject.SubjectCode}-{subject.Section}.");
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("SubjectRetirement")]
    public async Task<ActionResult<VStudentDetail>> SubjectRetirement(int subjectDetailID, int studentID)
    {
        try
        {
            if (!await _context.StudentSubjectValidation(subjectDetailID, studentID))
                return NotFound("Student is not taking the subject.");

            await _context.StudentSubjectElimination(studentID, subjectDetailID);

            return Ok("Subject successfully retired.");
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("SubjectElimination")]
    public async Task<ActionResult<VStudentDetail>> SubjectElimination(int subjectDetailID, int studentID)
    {
        try
        {
            if (!await _context.StudentSubjectValidation(subjectDetailID, studentID))
                return NotFound("Student is not taking the subject.");

            await _context.StudentSubjectElimination(subjectDetailID, studentID);

            return Ok("Subject successfully eliminated.");
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("Schedule")]
    public async Task<ActionResult<IEnumerable<VStudentSubject>>> GetStudentSchedule(int studentId)
    {
        try
        {
            var subjects = await _context.GetStudentSchedule(studentId);

            return !subjects.Any() ?
                NotFound("No subjects selected.") :
                Ok(subjects);
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("RequestGradeRevision")]
    public async Task<ActionResult<string>> RequestGradeRevision(int studentId, int subjectDetailID)
    {
        try
        {
            if (!await _context.RequestGradeRevision(studentId, subjectDetailID))
                return NotFound("Student is not taking the subject.");

            return Ok("Request sended successfully.");
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("Search")]
    public async Task<ActionResult<VStudentDetail>> GetStudent(int? studentId = null, string? docNo = null)
    {
        if (studentId is null)
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
        
        try
        {
            var student = await _context.VStudentDetails
                .FirstOrDefaultAsync(s => s.PersonId == studentId);

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
