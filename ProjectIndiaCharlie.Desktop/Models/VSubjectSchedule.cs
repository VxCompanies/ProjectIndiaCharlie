using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.Models
{
    public partial class VSubjectSchedule
    {
        public int SubjectDetailId { get; set; }
        public int WeekdayId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
