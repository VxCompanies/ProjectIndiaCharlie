using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
using ProjectIndiaCharlie.Core.Helpers;
using ProjectIndiaCharlie.Core.Models;

namespace ProjectIndiaCharlie.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ProjectIndiaCharlieContext _context;

        public AdminController(ProjectIndiaCharlieContext context) => _context = context;

        [HttpGet("Login")]
        public async Task<ActionResult<VAdministratorDetail>> AdminLogin(int adminId, string password)
        {
            try
            {
                var passwordSalt = await _context.GetPasswordSalt(adminId);

                if (string.IsNullOrWhiteSpace(passwordSalt))
                    return NotFound("Invalid admin ID.");

                var passwordHash = PasswordHelpers.GetPasswordHash(password, passwordSalt);

                var admin = await _context.AdminLogin(adminId, passwordHash);

                return admin is null ?
                    NotFound("Wrong admin ID or password.") :
                    Ok(admin);
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message);
            }
        }

        [HttpGet("CareersList")]
        public async Task<ActionResult<IEnumerable<VAvailableCareer>>> GetCareers() => Ok(await _context.VAvailableCareers
            .ToListAsync());

        [HttpGet("GetUnsolvedRevisions")]
        public async Task<ActionResult<IEnumerable<VGradeRevision>>> GetUnsolvedRevisions()
        {
            var revisions = await _context.GetUnsolvedRevisions();
            return !revisions.Any() ?
                NotFound("There are no unsolved revisions.") :
                Ok(revisions);
        }

        [HttpPost("ProcessGradeRevisions")]
        public async Task<ActionResult<IEnumerable<VGradeRevision>>> ProcessGradeRevisions(int studentID, int subjectDetailID, int modifiedgradeId, int adminId)
        {
            await _context.ProcessGradeRevision(studentID, subjectDetailID, modifiedgradeId, adminId);
            return Ok("Grade revision processed successfully.");
        }

        [HttpGet("GradesList")]
        public async Task<ActionResult<IEnumerable<VGrade>>> GetGrades() => Ok(await _context.VGrades.ToListAsync());
    }
}
