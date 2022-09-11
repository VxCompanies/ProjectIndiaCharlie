using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class SubjectDetail
    {
        public SubjectDetail()
        {
            StudentSubjects = new HashSet<StudentSubject>();
            SubjectClassrooms = new HashSet<SubjectClassroom>();
            SubjectSchedules = new HashSet<SubjectSchedule>();
        }

        public int SubjectDetailId { get; set; }
        public int SubjectId { get; set; }
        public int ProfessorId { get; set; }
        public int Section { get; set; }
        public int Trimester { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Professor Professor { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
        public virtual ICollection<SubjectClassroom> SubjectClassrooms { get; set; }
        public virtual ICollection<SubjectSchedule> SubjectSchedules { get; set; }
    }
}
