using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ProjectIndiaCharlie.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordHelpersController : ControllerBase
    {
        [HttpGet("GetPasswordHash/{password}")]
        public ActionResult<string> GetPasswordHash(string password, string? salt = null) => Ok(PasswordHelpers.GetPasswordHash(password, salt));

        [HttpGet("GetRandomPassword")]
        public ActionResult<string> GetRandomPassword() => Ok(PasswordHelpers.GetRandomPassword());
    }
}
