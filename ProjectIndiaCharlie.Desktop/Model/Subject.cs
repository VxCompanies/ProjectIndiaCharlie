using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Subject
    {
        public Subject()
        {
            SubjectClassrooms = new HashSet<SubjectClassroom>();
            SubjectDetails = new HashSet<SubjectDetail>();
        }

        public int SubjectId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Credits { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<SubjectClassroom> SubjectClassrooms { get; set; }
        public virtual ICollection<SubjectDetail> SubjectDetails { get; set; }
    }
}
