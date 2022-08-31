using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class PersonRole
    {
        public int PersonId { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Person? Person { get; set; }
        public virtual Role Role { get; set; } = null!;
    }
}
