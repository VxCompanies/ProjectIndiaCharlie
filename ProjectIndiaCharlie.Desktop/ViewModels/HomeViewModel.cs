using ProjectIndiaCharlie.Core.Models;
using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Desktop.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public VStudentDetail Student { get; private set; }

    public ObservableCollection<VStudentSubject> SubjectsList { get; set; }

    public GetPeopleAsyncCommand GetPeopleAsyncCommand { get; set; }

    public HomeViewModel()
    {
        Student = LogedStudent.Student!;
        SubjectsList = new();
        GetPeopleAsyncCommand = new();

        GetSubjectsList();
    }

    private async void GetSubjectsList()
    {
        SubjectsList.Clear();

        foreach (var subject in await AcademicService.GetStudentSubjects(Student.PersonId.ToString()))
            SubjectsList.Add(subject);
    }
}
