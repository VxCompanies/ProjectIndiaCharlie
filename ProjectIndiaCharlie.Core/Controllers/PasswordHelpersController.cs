using Microsoft.AspNetCore.Mvc;

namespace ProjectIndiaCharlie.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordHelpersController : ControllerBase
    {
        [HttpGet("GetHash")]
        public ActionResult<string> GetPasswordHash(string password, string? salt = null) => Ok(PasswordHelpers.GetPasswordHash(password, salt));

        [HttpGet("GenRandom")]
        public ActionResult<string> GenRandomPassword() => Ok(PasswordHelpers.GenRandomPassword());
    }
}
