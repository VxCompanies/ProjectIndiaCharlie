using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.Models
{
    public partial class Professor
    {
        public Professor()
        {
            Sections = new HashSet<Section>();
        }

        public int PersonId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<Section> Sections { get; set; }
    }
}
