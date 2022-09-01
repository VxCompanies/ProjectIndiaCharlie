using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.Models
{
    public partial class Role
    {
        public Role()
        {
            PersonRoles = new HashSet<PersonRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<PersonRole> PersonRoles { get; set; }
    }
}
