using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class SelectionViewModel : ViewModelBase
    {
        public ObservableCollection<VStudentSubject> SelectedSubjects { get; set; }
        public ObservableCollection<VSubjectSectionDetail> AvailableSubjects { get; set; }
        public ObservableCollection<VSubjectSectionDetail> SubjectSections { get; set; }

        public SelectionViewModel()
        {
            SelectedSubjects = new();
            AvailableSubjects = new();
            SubjectSections = new();

            GetSelectedSubjects();
        }

        private async void GetSelectedSubjects()
        {
            SelectedSubjects.Clear();

            foreach (var subject in await StudentService.GetSchedule(LogedStudent.Student!.PersonId))
                SelectedSubjects.Add(subject);
        }
    }
}
