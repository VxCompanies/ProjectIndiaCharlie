using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Coordinator
    {
        public Coordinator()
        {
            Careers = new HashSet<Career>();
        }

        public int PersonId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<Career> Careers { get; set; }
    }
}
