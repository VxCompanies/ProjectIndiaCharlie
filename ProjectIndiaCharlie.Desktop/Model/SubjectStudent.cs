using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class SubjectStudent
    {
        public int SubjectStudentId { get; set; }
        public int StudentId { get; set; }
        public string? Grade { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Student Student { get; set; } = null!;
    }
}
