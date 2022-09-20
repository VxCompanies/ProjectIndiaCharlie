using System;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public partial class VAvailableCareer
    {
        public int CareerId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Credits { get; set; }
        public int Year { get; set; }
    }
}
