using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Command;
using ProjectIndiaCharlie.Desktop.ViewModels.Store;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Desktop.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public string WelcomeMessage => $"Welcome, {(LogedPerson.Person == null ? "Tester Admin" : LogedPerson.Person.FirstName)}!";

    //public static decimal GeneralGrade => LogedPerson.Person.Student!.GeneralGrade;

    public ObservableCollection<Person> PeopleList { get; set; }

    public GetPeopleAsyncCommand GetPeopleAsyncCommand { get; set; }

    public HomeViewModel()
    {
        PeopleList = new();
        GetPeopleAsyncCommand = new();
    }
}
