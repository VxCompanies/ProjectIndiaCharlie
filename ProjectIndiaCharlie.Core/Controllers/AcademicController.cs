using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Models;

namespace ProjectIndiaCharlie.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicController : ControllerBase
    {
        private readonly ProjectIndiaCharlieContext _context;

        public AcademicController(ProjectIndiaCharlieContext context) => _context = context;

        [HttpPut("ChangePassword")]
        public async Task<ActionResult<string>> ChangePassword(int personId, string newPassword)
        {
            try
            {
                await _context.PasswordUpsert(personId, new(newPassword));

                return AcceptedAtAction("ChangePassword", (object)"Password changed successfully.");
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message);
            }
        }

        [HttpGet("GradesList")]
        public async Task<ActionResult<IEnumerable<VGrade>>> GetGrades() => Ok(await _context.VGrades.ToListAsync());
    }
}
