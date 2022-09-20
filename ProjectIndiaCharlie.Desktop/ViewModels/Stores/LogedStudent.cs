using ProjectIndiaCharlie.Desktop.Models;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Stores;

public static class LogedStudent
{
    public static VStudentDetail? Student { get; set; }
    public static IEnumerable<VStudentSubject>? StudentSubjects { get; set; }
}
