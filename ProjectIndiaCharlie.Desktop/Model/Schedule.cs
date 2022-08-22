using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Schedule
    {
        public int ScheduleId { get; set; }
        public int SubjectId { get; set; }
        public string Weekday { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

        public virtual SubjectDetail Subject { get; set; } = null!;
    }
}
