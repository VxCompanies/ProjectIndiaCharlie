using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Student
    {
        public Student()
        {
            SubjectStudents = new HashSet<SubjectStudent>();
        }

        public int PersonId { get; set; }
        public int CareerId { get; set; }
        public decimal GeneralGrade { get; set; }
        public decimal Trimester { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

        public virtual Career Career { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<SubjectStudent> SubjectStudents { get; set; }
    }
}
