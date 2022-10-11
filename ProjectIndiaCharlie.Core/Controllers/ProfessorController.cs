using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Helpers;
using ProjectIndiaCharlie.Core.Models;
using System.Data;

namespace ProjectIndiaCharlie.Core.Controllers;

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
                var flag = await _context.ProfessorValidation(newProfessor.PersonId);

                if (flag)
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

    [HttpGet("SubjectSections")]
    public async Task<ActionResult<IEnumerable<VSubjectSectionDetail>>> GetSubjectSections(int professorId)
    {
        try
        {
            var subjects = await _context.GetSubjectsOfProfessor(professorId);

            return !subjects.Any() ?
                NotFound("Professor does not have any subjects yet.") :
                Ok(subjects);
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpPost("PublishGrade")]
    public async Task<ActionResult<string>> PublishGrade(int studentId, int subjectDetailId, int grade)
    {
        try
        {
            if (!await _context.StudentSubjectValidation(studentId, subjectDetailId))
                return NotFound("The student is not taking the subject.");

            await _context.PublishGrade(studentId, subjectDetailId, grade);
            return CreatedAtAction("PublishGrade", "Grade published successfully.");
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet("GetStudentsOfSubjects")]
    public async Task<ActionResult<IEnumerable<VSubjectStudent>>> GetStudentsOfSubjects(int subjectDetailId)
    {
        var studentDetails = _context.VSubjectStudents
            .Where(s => s.SubjectDetailId == subjectDetailId)
            .ToListAsync();

        return (await studentDetails).Any() ?
            Ok(await studentDetails) :
            NotFound("There are no students in this section.");
    }
}
