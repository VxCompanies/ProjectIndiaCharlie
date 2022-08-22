using System;

namespace ProjectIndiaCharlie.Core.Models;

public partial class Person
{
    public int PersonId { get; set; }
    public string DocNo { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public virtual Coordinator? Coordinator { get; set; }
    public virtual Professor? Professor { get; set; }
    public virtual Student? Student { get; set; }
}
