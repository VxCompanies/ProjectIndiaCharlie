using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels;

public class SelectionViewModel : ViewModelBase
{
    private VSubjectSectionDetail _selectedScheduleSubject;
    public VSubjectSectionDetail SelectedScheduleSubject
    {
        get => _selectedScheduleSubject;
        set
        {
            _selectedScheduleSubject = value;
            OnPropertyChanged(nameof(SelectedScheduleSubject));
        }
    }
    
    private VSubjectSectionDetail _selectedSelectionSubject;
    public VSubjectSectionDetail SelectedSelectionSubject
    {
        get => _selectedSelectionSubject;
        set
        {
            _selectedSelectionSubject = value;
            OnPropertyChanged(nameof(SelectedSelectionSubject));
        }
    }

    private string _subjectSearch;
    public string SubjectSearch
    {
        get => _subjectSearch;
        set
        {
            _subjectSearch = value;
            UpdateSubjectSections(_subjectSearch);
            OnPropertyChanged(nameof(SubjectSearch));
        }
    }

    public ObservableCollection<VSubjectSectionDetail> SelectedSubjects { get; set; }
    public ObservableCollection<VSubjectSectionDetail> SubjectSections { get; set; }

    public QuitSubjectAsyncCommand QuitSubjectAsyncCommand { get; set; }
    public SelectSubjectAsyncCommand SelectSubjectAsyncCommand { get; set; }

    public SelectionViewModel()
    {
        SelectedSubjects = new();
        SubjectSections = new();

        QuitSubjectAsyncCommand = new();
        SelectSubjectAsyncCommand = new();

        GetSelectedSubjects();
        GetSelectionSubjects();
    }

    private async void GetSelectedSubjects()
    {
        SelectedSubjects.Clear();

        foreach (var subject in await StudentService.GetSelectionSchedule())
            SelectedSubjects.Add(subject);
    }

    private async void GetSelectionSubjects()
    {
        SubjectSections.Clear();

        foreach (var subject in await StudentService.GetSelectionSubjects())
            SubjectSections.Add(subject);
    }
    
    private async void UpdateSubjectSections(string subjectSearch)
    {
        if (string.IsNullOrWhiteSpace(subjectSearch))
        {
            GetSelectionSubjects();
            return;
        }

        SubjectSections.Clear();

        foreach (var subject in (await StudentService.GetSelectionSubjects())
            .Where(s => s.SubjectCode.ToLower().Contains(SubjectSearch.ToLower()) || s.SubjectName.ToLower().Contains(SubjectSearch.ToLower())))
            SubjectSections.Add(subject);
    }
}
