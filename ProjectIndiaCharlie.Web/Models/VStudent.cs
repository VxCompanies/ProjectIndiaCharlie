using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Web.Models
{
    public partial class VStudent
    {
        public int PersonId { get; set; }
        public string DocNo { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string FirstSurname { get; set; } = null!;
        public string? SecondSurname { get; set; }
        public string Gender { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public decimal GeneralIndex { get; set; }
        public decimal TrimestralIndex { get; set; }
        public int Trimester { get; set; }
        public DateTime EnrolementDate { get; set; }
        public string Career { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Pensum { get; set; }
    }
}
