﻿namespace ProjectIndiaCharlie.Core.Models
{
    public class PersonPassword
    {
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public PersonPassword(string password, string passwordHash, string passwordSalt)
        {
            Password = password;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
