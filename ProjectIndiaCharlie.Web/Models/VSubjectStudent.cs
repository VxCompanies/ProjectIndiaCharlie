using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Web.Models
{
    public partial class VSubjectStudent
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int SubjectDetailId { get; set; }
        public string SubjectCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Section { get; set; }
        public string? Grade { get; set; }
        public decimal? Points { get; set; }
        public int ProfessorId { get; set; }
    }
}
