using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
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

            _ = GetSelectedSubjects();
            _ = GetSelectionSubjects();
        }

        private async Task GetSelectedSubjects()
        {
            SelectedSubjects.Clear();

            foreach (var subject in await StudentService.GetSelectionSchedule())
                SelectedSubjects.Add(subject);
        }

        private async Task GetSelectionSubjects()
        {
            SubjectSections.Clear();

            foreach (var subject in await StudentService.GetSelectionSubjects())
                SubjectSections.Add(subject);
        }
    }
}
