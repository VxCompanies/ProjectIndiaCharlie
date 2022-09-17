using ProjectIndiaCharlie.Desktop.Models;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Store;

public static class LogedStudent
{
    public static VStudentDetail? Student { get; set; }
    public static IEnumerable<SubjectStudent> StudentSubjects { get; set; }
}
