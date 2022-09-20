using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Data;
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
        public async Task<ActionResult<VProfessorDetail>> AdminLogin(int adminId, string password)
        {
            try
            {
                var passwordSalt = await _context.GetPasswordSalt(adminId);

                if (string.IsNullOrWhiteSpace(passwordSalt))
                    return NotFound();

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
        public async Task<ActionResult<IEnumerable<VAvailableCareer>>> GetCareers() => await _context.VAvailableCareers.ToListAsync();
    }
}
