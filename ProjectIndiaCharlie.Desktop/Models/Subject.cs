﻿using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Sections = new HashSet<Section>();
        }

        public int SubjectId { get; set; }
        public int AreaId { get; set; }
        public string SubjectCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public byte Credits { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Area Area { get; set; } = null!;
        public virtual ICollection<Section> Sections { get; set; }
    }
}
