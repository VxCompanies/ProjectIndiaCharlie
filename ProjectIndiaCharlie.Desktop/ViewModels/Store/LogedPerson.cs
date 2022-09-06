using ProjectIndiaCharlie.Desktop.Models;
using System.Collections.Generic;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Store;

public static class LogedPerson
{
    public static Person? Person { get; set; }
    public static IEnumerable<SubjectStudent> SubjectStudent { get; set; }
}
