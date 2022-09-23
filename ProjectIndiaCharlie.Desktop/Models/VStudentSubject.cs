using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.Models
{
    public partial class VStudentSubject
    {
        public int StudentId { get; set; }
        public int SubjectDetailId { get; set; }
        public string SubjectCode { get; set; } = null!;
        public int Section { get; set; }
        public string Subject { get; set; } = null!;
        public string Professor { get; set; } = null!;
        public byte Credits { get; set; }
        public string ClassroomCode { get; set; } = null!;
        public int Trimester { get; set; }
        public int Year { get; set; }
        public string? Monday { get; set; }
        public string? Tuesday { get; set; }
        public string? Wednesday { get; set; }
        public string? Thursday { get; set; }
        public string? Friday { get; set; }
        public string? Saturday { get; set; }
        public string? Grade { get; set; }
        public decimal? Points { get; set; }
    }
}
