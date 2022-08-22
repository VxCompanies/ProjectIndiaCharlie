using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class SubjectClassroom
    {
        public int SubjectId { get; set; }
        public int ClassroomId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Classroom Classroom { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
    }
}
