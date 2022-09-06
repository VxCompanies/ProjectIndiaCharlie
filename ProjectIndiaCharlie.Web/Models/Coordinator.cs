using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Web.Models
{
    public partial class Coordinator
    {
        public Coordinator()
        {
            Areas = new HashSet<Area>();
            Careers = new HashSet<Career>();
        }

        public int PersonId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Career> Careers { get; set; }
    }
}
