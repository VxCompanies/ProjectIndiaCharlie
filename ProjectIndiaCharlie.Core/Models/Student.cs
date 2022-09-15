using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentSubjects = new HashSet<StudentSubject>();
        }

        public int PersonId { get; set; }
        public int CareerId { get; set; }
        public decimal GeneralIndex { get; set; }
        public decimal TrimestralIndex { get; set; }
        public int Trimester { get; set; }
        public DateTime EnrollementDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Career Career { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
