using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.WebAdministrator.Models
{
    public partial class VGradeRevision
    {
        public int PersonId { get; set; }
        public string Student { get; set; } = null!;
        public int SubjectDetailId { get; set; }
        public string Section { get; set; } = null!;
        public DateTime DateRequested { get; set; }
        public int GradeId { get; set; }
        public string Grade { get; set; } = null!;
        public int? ModifiedGradeId { get; set; }
        public int? ModifiedGrade { get; set; }
        public int? Admin { get; set; }
        public int? Professor { get; set; }
    }
}
