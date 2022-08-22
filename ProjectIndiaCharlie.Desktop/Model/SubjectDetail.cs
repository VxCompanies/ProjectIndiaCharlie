using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class SubjectDetail
    {
        public SubjectDetail()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int SubjectDetailId { get; set; }
        public int ProfessorId { get; set; }
        public int SubjectId { get; set; }
        public string Section { get; set; } = null!;
        public string Trimester { get; set; } = null!;
        public string Year { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

        public virtual Professor Professor { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
