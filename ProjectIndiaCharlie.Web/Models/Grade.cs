using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Web.Models
{
    public partial class Grade
    {
        public int GradeId { get; set; }
        public string Grade1 { get; set; } = null!;
        public double Points { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
