using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Models;

namespace ProjectIndiaCharlie.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly ProjectIndiaCharlieContext _context;

    public PeopleController(ProjectIndiaCharlieContext context) => _context = context;

    [HttpGet("GetPeople")]
    public async Task<ActionResult<IEnumerable<Person>>> GetPeople() =>
        _context.People == null ?
        NotFound() :
        Ok(await _context.People.ToListAsync());

    [HttpGet("GetPerson/{id}")]
    public async Task<ActionResult<Person?>> GetPerson(int id) => await _context.People.FirstOrDefaultAsync(p => p.PersonId == id);

    [HttpPost("RegisterPerson")]
    public async Task<ActionResult<Person>> RegisterPerson(Person person)
    {
        if (await _context.People.FirstOrDefaultAsync(s => s.DocNo != person.DocNo) != null)
            return Conflict("Persona con el número de cédula ingresado ya existe.");

        _context.Add(person);
        await _context.SaveChangesAsync();

        return CreatedAtAction("RegisterPerson", person);
    }

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

        var passwordHash = new PasswordHelpersController().GetPasswordHash($"{password}{user.PersonPassword!.PasswordSalt}");

        if (user.PersonPassword.PasswordHash != passwordHash.Value)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("GetStudents")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents() =>
        (_context.Students is null) ?
        NotFound() :
        await _context.Students.Include(p => p.Person)
        .ToListAsync();

    [HttpGet("GetStudent")]
    public async Task<ActionResult<Student>> GetStudent(int personId)
    {
        var student = await _context.Students.Include(s => s.Career)
            .FirstOrDefaultAsync(s => s.PersonId == personId);

        return (student is null) ?
            NotFound() :
            Ok(student);
    }

    //[HttpPut]
    //public async Task<IActionResult> PutStudent(int id, Student student)
    //{
    //    if (id != student.PersonId)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(student).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!StudentExists(id))
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
    //            throw;
    //        }
    //    }

    //    return NoContent();
    //}

    [HttpPost("RegisterStudent")]
    public async Task<ActionResult<Student>> RegisterStudent(Student student)
    {
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
            await _context.Database.ExecuteSqlRawAsync("Person.SP_CreateUser", spParams);
        }
        catch (DbUpdateException e)
        {
            return Problem(detail: e.Message);
        }

        return CreatedAtAction("RegisterStudent", student);
    }
}
