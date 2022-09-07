using System.Security.Cryptography;
using System.Text;

namespace ProjectIndiaCharlie.Core.Controllers
{
    public static class PasswordHelpers
    {
        public static string GetPasswordHash(string password, string? salt)
        {
            var sha256 = SHA256.Create();

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{password}{salt ?? string.Empty}"));

            var hash = string.Empty;

            for (int i = 0; i < bytes.Length; i++)
                hash += bytes[i].ToString("x2");

            return hash;
        }

        public static string GetRandomPassword()
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
