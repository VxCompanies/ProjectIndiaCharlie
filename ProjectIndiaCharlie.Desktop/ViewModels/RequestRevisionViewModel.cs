using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands.Navigation;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels;

public class RequestRevisionViewModel : ViewModelBase
{
    private VSubjectStudent _selectedSubject;
    public VSubjectStudent SelectedSubject
    {
        get => _selectedSubject;
        set
        {
            _selectedSubject = value;
            OnPropertyChanged(nameof(SelectedSubject));
        }
    }

    public ObservableCollection<VSubjectStudent> SelectedSubjects { get; set; }

    public RequestRevisionAsyncCommand RequestRevisionAsyncCommand { get; set; }

    public NavigatePendingRevisionsCommand NavigatePendingRevisionsCommand { get; set; }

    public RequestRevisionViewModel()
    {
        SelectedSubjects = new();
        RequestRevisionAsyncCommand = new();
        NavigatePendingRevisionsCommand = new();

        GetSchedule();
    }

    private async void GetSchedule()
    {
        SelectedSubjects.Clear();

        foreach (var subject in await StudentService.GetRetirableSubjects())
            SelectedSubjects.Add(subject);
    }
}
