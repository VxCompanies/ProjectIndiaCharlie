using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Command;
using ProjectIndiaCharlie.Desktop.ViewModels.Service;
using ProjectIndiaCharlie.Desktop.ViewModels.Store;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Desktop.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public Person LogedUser { get; private set; }

    public ObservableCollection<SubjectStudent> SubjectsList { get; set; }

    public GetPeopleAsyncCommand GetPeopleAsyncCommand { get; set; }

    public HomeViewModel()
    {
        LogedUser = LogedPerson.Person!;
        SubjectsList = new();
        GetPeopleAsyncCommand = new();

        GetSubjectsList();
    }

    private async void GetSubjectsList()
    {
        SubjectsList.Clear();

        foreach (var subject in await AcademicService.GetStudentSubjects(LogedUser.PersonId.ToString()))
            SubjectsList.Add(subject);
    }
}
