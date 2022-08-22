using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("GetPerson")]
    public async Task<ActionResult<Person>> GetPerson(string search, SearchCriteria searchCriteria)
    {
        if (_context.People == null)
            return NotFound();

        return searchCriteria switch
        {
            SearchCriteria.id => (!int.TryParse(search, out int id)) ?
                                BadRequest() :
                                (await _context.People.FindAsync(id) == null) ?
                                NotFound() :
                                Ok(await _context.People.FindAsync(id)),
            SearchCriteria.docNo => (await _context.People.FirstOrDefaultAsync(p => p.DocNo == search) == null) ?
                                NotFound() :
                                Ok(await _context.People.FirstOrDefaultAsync(p => p.DocNo == search)),
            SearchCriteria.firstName => (await _context.People.FirstOrDefaultAsync(p => p.FirstName == search) == null) ?
                                NotFound() :
                                Ok(await _context.People.FirstOrDefaultAsync(p => p.FirstName == search)),
            SearchCriteria.middleName => (await _context.People.FirstOrDefaultAsync(p => p.MiddleName == search) == null) ?
                                NotFound() :
                                Ok(await _context.People.FirstOrDefaultAsync(p => p.MiddleName == search)),
            SearchCriteria.lastName => (await _context.People.FirstOrDefaultAsync(p => p.LastName == search) == null) ?
                                NotFound() :
                                Ok(await _context.People.FirstOrDefaultAsync(p => p.LastName == search)),
            _ => (ActionResult<Person>)BadRequest(),
        };
    }

    [HttpPost("RegisterPerson")]
    public async Task<ActionResult<Person>> RegisterPerson(Person person)
    {
        if (await _context.People.FirstOrDefaultAsync(s => s.DocNo != person.DocNo) != null)
            return Conflict("Persona con el número de cédula ingresado ya existe.");

        _context.Add(person);
        await _context.SaveChangesAsync();

        return CreatedAtAction("RegisterPerson", person);
    }

    [HttpPost("RegisterStudent")]
    public async Task<ActionResult<Student>> RegisterStudent(Student student)
    {
        if (student == null)
            return BadRequest("Student cannot be null.");

        var p = await _context.People.FirstOrDefaultAsync(s => s.DocNo != student.Person.DocNo);

        if (p != null)
            student.Person = p;

        try
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }

        return Ok(student);
    }
}
