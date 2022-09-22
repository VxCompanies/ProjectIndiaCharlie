using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
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
        public string? ModifiedGrade { get; set; }
        public int AdminId { get; set; }
        public string? Admin { get; set; }
        public int ProfessorId { get; set; }
        public string? Professor { get; set; }
    }
}
