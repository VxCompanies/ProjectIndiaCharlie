﻿namespace ProjectIndiaCharlie.Core.Models
{
    public class NewPerson
    {
        public string DocNo { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string FirstSurname { get; set; } = null!;
        public string? SecondSurname { get; set; }
        public string Gender { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public int? CareerId { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
    }
}
