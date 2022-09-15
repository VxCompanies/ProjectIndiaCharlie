using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Models;

namespace ProjectIndiaCharlie.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ProjectIndiaCharlieContext _context;

        public PersonController(ProjectIndiaCharlieContext context) => _context = context;

        [HttpGet("Search")]
        public async Task<ActionResult<VPeopleDetail>> GetPerson(string docNo)
        {
            var person = await _context.VPeopleDetails
                .FirstOrDefaultAsync(s => s.DocNo == docNo);

            return (person is null) ?
                NotFound() :
                Ok(person);
        }
    }
}
