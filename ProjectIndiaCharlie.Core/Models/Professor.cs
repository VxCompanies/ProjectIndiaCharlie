﻿using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Professor
    {
        public Professor()
        {
            SubjectDetails = new HashSet<SubjectDetail>();
        }

        public int PersonId { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<SubjectDetail> SubjectDetails { get; set; }
    }
}
