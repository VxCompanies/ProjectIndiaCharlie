using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class SubjectClassroom
    {
        public int SubjectDetailId { get; set; }
        public int ClassroomId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Classroom Classroom { get; set; } = null!;
        public virtual Section SubjectDetail { get; set; } = null!;
    }
}
