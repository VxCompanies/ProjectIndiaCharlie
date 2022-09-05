using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Models;
using System.Data;

namespace ProjectIndiaCharlie.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly ProjectIndiaCharlieContext _context;

    public PersonController(ProjectIndiaCharlieContext context) => _context = context;

    #region People
    [HttpGet("GetPeople")]
    public async Task<ActionResult<IEnumerable<Person>>> GetPeople() =>
        _context.People == null ?
        NotFound() :
        Ok(await _context.People
            .Include(p => p.Professor)
            .Include(c => c.Coordinator)
            .Include(p => p.PersonPassword)
            .ToListAsync());

    [HttpGet("GetPerson")]
    public async Task<ActionResult<Person?>> GetPerson(int personId) => await _context.People.FirstOrDefaultAsync(p => p.PersonId == personId);

    #endregion

    [HttpGet("Login")]
    public async Task<ActionResult<Person>> Login(int personId, string password)
    {
        var user = await _context.People.Include(s => s.Student)
            .Include(p => p.Professor)
            .Include(c => c.Coordinator)
            .Include(p => p.PersonPassword)
            .FirstOrDefaultAsync(p => p.PersonId == personId);

        if (user is null)
            return NotFound();

        var passwordHash = PasswordHelpers.GetPasswordHash(password, user.PersonPassword!.PasswordSalt);

        //if (_context.People.FromSqlRaw("Person.F_Login", personId, passwordHash) is null)
        //    return NotFound();

        return (user.PersonPassword.PasswordHash == passwordHash) ?
            Ok(user) :
            NotFound();
    }

    #region Student
    [HttpPost("Student/Registration")]
    public async Task<ActionResult<Student>> RegisterStudent(Student student)
    {
        var tran = _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

        if (await _context.Students.FindAsync(student.PersonId) != null)
            return Conflict();

        if (await _context.People.AnyAsync(p => p.DocNo == student.Person.DocNo))
            student.Person = new();

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
            await tran;
            await _context.Database.ExecuteSqlRawAsync("Person.SP_RegisterPerson", spParams);
            await _context.Database.CommitTransactionAsync();
        }
        catch (DbUpdateException e)
        {
            return Problem(detail: e.Message);
        }

        return CreatedAtAction("RegisterStudent", student);
    }

    [HttpGet("Students")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents() =>
        (_context.Students is null) ?
        NotFound() :
        Ok(await _context.Students.Include(p => p.Person)
        .ToListAsync());

    [HttpGet("GetStudent/{personId}")]
    public async Task<ActionResult<Student>> GetStudent(int personId)
    {
        var student = await _context.Students.Include(s => s.Career)
            .FirstOrDefaultAsync(s => s.PersonId == personId);

        return (student is null) ?
            NotFound() :
            Ok(student);
    }
    #endregion

    #region Proffessor
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
    #endregion

    [HttpPost("Coordinator/Registration")]
    public async Task<ActionResult<Student>> RegisterCoordinator(Coordinator coordinator)
    {
        var tran = _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

        if (await _context.Coordinators.FindAsync(coordinator.PersonId) != null)
            return Conflict();

        if (await _context.People.AnyAsync(p => p.DocNo == coordinator.Person.DocNo))
            coordinator.Person = new();

        var spParams = new List<SqlParameter>
        {
            new("@DocNo", coordinator.Person.DocNo),
            new("@FirstName", coordinator.Person.FirstName),
            new("@MiddleName", coordinator.Person.MiddleName),
            new("@FirstSurname", coordinator.Person.FirstSurname),
            new("@SecondSurname", coordinator.Person.SecondSurname),
            new("@Gender", coordinator.Person.Gender),
            new("@BirthDate", coordinator.Person.BirthDate),
            new("@Email", coordinator.Person.Email),
            new("@RolId", 3),
            new("@PasswordHash", coordinator.Person.PersonPassword!.PasswordHash),
            new("@PasswordSalt", coordinator.Person.PersonPassword!.PasswordSalt),
        };

        try
        {
            await tran;
            await _context.Database.ExecuteSqlRawAsync("Person.SP_RegisterPerson", spParams);
            await _context.Database.CommitTransactionAsync();
        }
        catch (DbUpdateException e)
        {
            return Problem(detail: e.Message);
        }

        return CreatedAtAction("RegisterCoordinator", coordinator);
    }
}
