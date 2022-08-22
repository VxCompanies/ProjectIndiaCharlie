using ProjectIndiaCharlie.Core.Models;
using ProjectIndiaCharlie.Desktop.ViewModel.Command;
using ProjectIndiaCharlie.Desktop.ViewModel.Store;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Desktop.ViewModel;

public class HomeViewModel : ViewModelBase
{
    public string WelcomeMessage => $"Welcome, {(LogedPerson.Person == null ? "Tester Admin" : LogedPerson.Person.FirstName)}!";

    public static decimal GeneralGrade => LogedPerson.Person.Student!.GeneralGrade;

    public ObservableCollection<Person> PeopleList { get; set; }

    public GetPeopleAsyncCommand GetPeopleAsyncCommand { get; set; }

    public HomeViewModel()
    {
        PeopleList = new();
        GetPeopleAsyncCommand = new();
    }
}
