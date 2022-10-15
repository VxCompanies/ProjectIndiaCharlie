using ProjectIndiaCharlie.Mobile.Models;
using ProjectIndiaCharlie.Mobile.ViewModels.Services;
using ProjectIndiaCharlie.Mobile.ViewModels.Stores;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Mobile.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public VStudentDetail Student { get; set; }
    public string Name { get; set; }
    public string Career { get; set; }

    public ObservableCollection<VStudentSubject> Schedule { get; set; }

    public HomeViewModel()
    {
        Schedule = new();
        Student = LogedStudent.Student!;

        LoadSchedule();
        LoadStudentInfo();
    }

    private async void LoadSchedule()
    {
        foreach (var subject in await StudentService.GetSchedule())
            Schedule.Add(subject);
    }

    private void LoadStudentInfo()
    {
        Name = $"{Student.FirstName} {Student.MiddleName} {Student.FirstSurname} {Student.SecondSurname}";
        Career = $"{Student.CareerCode} - {Student.Career} ({Student.Pensum})";
    }
}
