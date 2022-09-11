﻿using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Weekday
    {
        public Weekday()
        {
            SubjectSchedules = new HashSet<SubjectSchedule>();
        }

        public int WeekdayId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<SubjectSchedule> SubjectSchedules { get; set; }
    }
}
