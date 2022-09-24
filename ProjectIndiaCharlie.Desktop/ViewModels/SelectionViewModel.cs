using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class SelectionViewModel : ViewModelBase
    {
        public ObservableCollection<VSubjectSectionDetail> SelectedSubjects { get; set; }
        public ObservableCollection<VSubjectSectionDetail> SubjectSections { get; set; }

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

        public SelectionViewModel()
        {
            SelectedSubjects = new();
            SubjectSections = new();

            GetSelectedSubjects();
            GetSelectionSubjects();
        }

        private async void GetSelectedSubjects()
        {
            SelectedSubjects.Clear();

            foreach (var subject in await StudentService.GetSelectionSchedule(LogedStudent.Student!.PersonId))
                SelectedSubjects.Add(subject);
        }

        private async void GetSelectionSubjects()
        {
            SubjectSections.Clear();

            foreach (var subject in await StudentService.GetSelectionSubjects())
                SubjectSections.Add(subject);
        }
    }
}
