using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Section
    {
        public Section()
        {
            Schedules = new HashSet<Schedule>();
            SubjectClassrooms = new HashSet<SubjectClassroom>();
            SubjectStudents = new HashSet<SubjectStudent>();
        }

        public int SubjectDetailId { get; set; }
        public int SubjectId { get; set; }
        public int ProfessorId { get; set; }
        public int Section1 { get; set; }
        public int Trimester { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Professor? Professor { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<SubjectClassroom> SubjectClassrooms { get; set; }
        public virtual ICollection<SubjectStudent> SubjectStudents { get; set; }
    }
}
