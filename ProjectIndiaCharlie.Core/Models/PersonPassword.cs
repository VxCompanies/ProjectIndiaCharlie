namespace ProjectIndiaCharlie.Core.Models;

public class PersonPassword
{
    public string Password { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;

    public PersonPassword(string password, string passwordHash, string passwordSalt)
    {
        Password = password;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }
}
