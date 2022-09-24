using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Helpers;
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
                var flag = await _context.StudentValidation(newStudent.PersonId);

                if (flag)
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
                NotFound("Wrong student Id or password.") :
                Ok(student);
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpPost("SubjectSelection")]
    public async Task<ActionResult<string>> SubjectSelection(int studentId, int subjectDetailId)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var scheduleValidation = await _context.SubjectScheduleValidation(studentId, subjectDetailId);
            if (!string.IsNullOrWhiteSpace(scheduleValidation))
            {
                await _context.Database.RollbackTransactionAsync();
                return Conflict($"Cannot select this subject because the schedule conflicts with '{scheduleValidation}'.");
            }

            if (!await _context.SubjectSelection(studentId, subjectDetailId))
                return Conflict($"Subject section is full.");

            await _context.Database.CommitTransactionAsync();

            var subject = await _context.VStudentSubjects
                            .FirstAsync(s => s.SubjectDetailId == subjectDetailId);
            return base.CreatedAtAction("SubjectSelection", $"Successfully selected {subject.SubjectCode}-{subject.Section}.");
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            return Problem(detail: e.Message);
        }
    }

    [HttpPut("SubjectRetirement")]
    public async Task<ActionResult<string>> SubjectRetirement(int studentId, int subjectDetailId)
    {
        try
        {
            if (!await _context.StudentSubjectValidation(studentId, subjectDetailId))
                return NotFound("Student is not taking the subject.");

            await _context.SubjectRetirement(studentId, subjectDetailId);

            return Ok("Subject successfully retired.");
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpDelete("SubjectElimination")]
    public async Task<ActionResult<string>> SubjectElimination(int studentId, int subjectDetailId)
    {
        try
        {
            if (!await _context.StudentSubjectValidation(studentId, subjectDetailId))
                return NotFound("Student is not taking the subject.");

            await _context.StudentSubjectElimination(studentId, subjectDetailId);

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

    [HttpPost("RequestGradeRevision")]
    public async Task<ActionResult<string>> RequestGradeRevision(int studentId, int subjectDetailId)
    {
        try
        {
            if (!await _context.RequestGradeRevision(studentId, subjectDetailId))
                return NotFound("Student is not taking the subject.");

            return Ok("Request sended successfully.");
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    // TODO: GetGrades
    [HttpGet("GetGrades")]
    public async Task<ActionResult<IEnumerable<object>>> GetGrades(int studentId, int year, int trimester)
    {
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

    [HttpGet("GetSelectionSchedule")]
    public async Task<ActionResult<IEnumerable<VSubjectSectionDetail>>> GetSelectionSchedule(int studentId)
    {
        try
        {
            var selectionSchedule = await _context.GetSelectionSchedule(studentId);

            return (!selectionSchedule.Any()) ?
                NotFound("No subjects selected.") :
                Ok(selectionSchedule);
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("GetSelectionSubjects")]
    public async Task<ActionResult<IEnumerable<VSubjectSectionDetail>>> GetSelectionSubjects()
    {
        try
        {
            var student = await _context.VSubjectSectionDetails
                .ToListAsync();

            return (!student.Any()) ?
                NotFound("There are no subjects for this trimester selection.") :
                Ok(student);
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
