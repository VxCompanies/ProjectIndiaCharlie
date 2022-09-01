using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.Models
{
    public partial class Building
    {
        public Building()
        {
            Classrooms = new HashSet<Classroom>();
        }

        public int BuildingId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}
