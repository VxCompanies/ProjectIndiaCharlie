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
        public ActionResult<string> GetPasswordHash(string password, string? salt = null)
        {
            var sha256 = SHA256.Create();

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{password}{salt ?? string.Empty}"));

            var hash = string.Empty;

            for (int i = 0; i < bytes.Length; i++)
                hash += bytes[i].ToString("x2");

            return hash;
        }

        [HttpGet("GetRandomPassword")]
        public ActionResult<string> GetRandomPassword()
        {
            var ran = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var pass = string.Empty;

            for (int i = 0; i < 12; i++)
               pass += chars[ran.Next(chars.Length)];

            return pass;
        }
    }
}
