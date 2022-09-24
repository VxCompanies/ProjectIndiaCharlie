using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class SelectionViewModel : ViewModelBase
    {
        private VSubjectSectionDetail _selectedSubject;
        public VSubjectSectionDetail SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
            }
        }

        public ObservableCollection<VSubjectSectionDetail> SelectedSubjects { get; set; }
        public ObservableCollection<VSubjectSectionDetail> SubjectSections { get; set; }

        public QuitSubjectAsyncCommand QuitSubjectAsyncCommand { get; set; }

        public SelectionViewModel()
        {
            SelectedSubjects = new();
            SubjectSections = new();

            QuitSubjectAsyncCommand = new();

            GetSelectedSubjects();
            GetSelectionSubjects();
        }

        public async void GetSelectedSubjects()
        {
            SelectedSubjects.Clear();

            foreach (var subject in await StudentService.GetSelectionSchedule())
                SelectedSubjects.Add(subject);
        }

        public async void GetSelectionSubjects()
        {
            SubjectSections.Clear();

            foreach (var subject in await StudentService.GetSelectionSubjects())
                SubjectSections.Add(subject);
        }
    }
}
