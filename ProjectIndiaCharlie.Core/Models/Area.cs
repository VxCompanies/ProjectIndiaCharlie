using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Area
    {
        public Area()
        {
            Subjects = new HashSet<Subject>();
        }

        public int AreaId { get; set; }
        public int CoordinatorId { get; set; }
        public string AreaCode { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Coordinator Coordinator { get; set; } = null!;
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
