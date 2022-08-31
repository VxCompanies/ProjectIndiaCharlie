using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class Schedule
    {
        public int ScheduleId { get; set; }
        public int SubjectDetailId { get; set; }
        public int WeekdayId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Section SubjectDetail { get; set; } = null!;
        public virtual Weekday Weekday { get; set; } = null!;
    }
}
