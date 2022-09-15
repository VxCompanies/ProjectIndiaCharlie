using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class SubjectSchedule
    {
        public int SubjectScheduleId { get; set; }
        public int SubjectDetailId { get; set; }
        public int WeekdayId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual SubjectDetail? SubjectDetail { get; set; }
        public virtual Weekday? Weekday { get; set; }
    }
}
