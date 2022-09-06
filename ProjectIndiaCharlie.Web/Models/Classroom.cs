using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Web.Models
{
    public partial class Classroom
    {
        public Classroom()
        {
            SubjectClassrooms = new HashSet<SubjectClassroom>();
        }

        public int ClassroomId { get; set; }
        public int Code { get; set; }
        public int BuildingId { get; set; }
        public bool IsLab { get; set; }
        public int Capacity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Building Building { get; set; } = null!;
        public virtual ICollection<SubjectClassroom> SubjectClassrooms { get; set; }
    }
}
