using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands.Navigation;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels;

public class PendingRevisionViewModel : ViewModelBase
{
    public ObservableCollection<VGradeRevision> PendingRevisions { get; set; }

    public NavigateRequestRevisionCommand NavigateRequestRevisionCommand { get; set; }

    public PendingRevisionViewModel()
    {
        PendingRevisions = new();
        NavigateRequestRevisionCommand = new();

        _ = GetSchedule();
    }

    private async Task GetSchedule()
    {
        PendingRevisions.Clear();

        foreach (var subject in await StudentService.GetPendingGradeRequests())
            PendingRevisions.Add(subject);
    }
}
