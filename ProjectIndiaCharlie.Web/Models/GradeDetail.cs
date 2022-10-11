namespace ProjectIndiaCharlie.Web.Models
{
    public class GradeDetail
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int SubjectDetailId { get; set; }
        public int Grade { get; set; }

        public GradeDetail()
        {
        }

        public GradeDetail(VSubjectStudent student)
        {
            StudentId = student.StudentId;
            StudentName = student.StudentName;
            SubjectDetailId = student.SubjectDetailId;
        }
    }
}
