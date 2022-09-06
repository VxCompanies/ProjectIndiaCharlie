using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Web.Models
{
    public partial class Weekday
    {
        public Weekday()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int WeekdayId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
