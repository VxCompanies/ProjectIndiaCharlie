using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Desktop.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public VStudentDetail Student { get; private set; }

    public ObservableCollection<VStudentSubject> SelectedSubjectsList { get; set; }

    public HomeViewModel()
    {
        Student = LogedStudent.Student!;
        SelectedSubjectsList = new();

        GetSubjectsList();
    }

    private async void GetSubjectsList()
    {
        SelectedSubjectsList.Clear();

        foreach (var subject in await StudentService.GetSelectedSubjects(Student.PersonId))
            SelectedSubjectsList.Add(subject);
    }
}
