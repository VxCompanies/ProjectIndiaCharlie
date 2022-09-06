using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Web.Models
{
    public partial class Career
    {
        public Career()
        {
            Students = new HashSet<Student>();
        }

        public int CareerId { get; set; }
        public int CoordinatorId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Subjects { get; set; }
        public int Credits { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Coordinator Coordinator { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; }
    }
}
