using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Person
    {
        public Person()
        {
            PersonRoles = new HashSet<PersonRole>();
        }

        public int PersonId { get; set; }
        public string DocNo { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string FirstSurname { get; set; } = null!;
        public string? SecondSurname { get; set; }
        public string Gender { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Coordinator? Coordinator { get; set; }
        public virtual PersonPassword? PersonPassword { get; set; }
        public virtual Professor? Professor { get; set; }
        public virtual Student? Student { get; set; }
        public virtual ICollection<PersonRole> PersonRoles { get; set; }
    }
}
