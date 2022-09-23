using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;

namespace ProjectIndiaCharlie.Desktop.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public VStudentDetail Student => LogedStudent.Student!;
    public string Names => $"{Student.FirstName} {Student.MiddleName}";
    public string LastNames => $"{Student.FirstSurname} {Student.SecondSurname}";
    public string Career => $"{Student.CareerCode} {Student.Pensum} - {Student.Career}";
    public decimal TrimestralIndexPercentage => Student.TrimestralIndex * 25;
    public decimal GeneralIndexPercentage => Student.GeneralIndex * 25;
    public decimal TrimesterPercentage => Student.Trimester * (50/7);
}
