namespace ProjectIndiaCharlie.Core.Models
{
    public class NewCareer
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Subjects { get; set; }
        public int Credits { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
    }
}
