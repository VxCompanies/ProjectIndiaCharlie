using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Classroom
    {
        public Classroom()
        {
            SubjectClassrooms = new HashSet<SubjectClassroom>();
        }

        public int ClassroomId { get; set; }
        public string Code { get; set; } = null!;
        public int BuildingId { get; set; }
        public bool IsLab { get; set; }
        public decimal Capacity { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Building Building { get; set; } = null!;
        public virtual ICollection<SubjectClassroom> SubjectClassrooms { get; set; }
    }
}
