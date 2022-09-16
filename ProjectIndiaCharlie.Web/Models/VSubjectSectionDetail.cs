﻿using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Web.Models
{
    public partial class VSubjectSectionDetail
    {
        public int SubjectDetailId { get; set; }
        public string SubjectCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public byte Credits { get; set; }
        public int Section { get; set; }
        public string Professor { get; set; } = null!;
        public string Capacity { get; set; } = null!;
        public string ClassroomCode { get; set; } = null!;
        public string? Monday { get; set; }
        public string? Tuesday { get; set; }
        public string? Wednesday { get; set; }
        public string? Thursday { get; set; }
        public string? Friday { get; set; }
        public string? Saturday { get; set; }
    }
}