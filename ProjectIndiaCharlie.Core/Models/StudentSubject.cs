using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class StudentSubject
    {
        public int SubjectDetailId { get; set; }
        public int StudentId { get; set; }
        public int? GradeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Student Student { get; set; } = null!;
        public virtual SubjectDetail SubjectDetail { get; set; } = null!;
    }
}
