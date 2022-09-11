﻿using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Subject
    {
        public Subject()
        {
            SubjectDetails = new HashSet<SubjectDetail>();
        }

        public int SubjectId { get; set; }
        public string SubjectCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public byte Credits { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<SubjectDetail> SubjectDetails { get; set; }
    }
}
