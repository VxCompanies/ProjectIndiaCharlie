﻿using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Classroom
    {
        public Classroom()
        {
            SubjectClassrooms = new HashSet<SubjectClassroom>();
        }

        public int ClassroomId { get; set; }
        public string Code { get; set; } = null!;
        public bool IsLab { get; set; }
        public int Capacity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<SubjectClassroom> SubjectClassrooms { get; set; }
    }
}
