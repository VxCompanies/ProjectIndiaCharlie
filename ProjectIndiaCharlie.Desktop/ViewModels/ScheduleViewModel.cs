using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        public ObservableCollection<VStudentSubject> Schedule { get; set; }

        public ScheduleViewModel()
        {
            Schedule = new();
            GetSchedule();
        }

        private async void GetSchedule()
        {
            Schedule.Clear();

            foreach (var subject in (await StudentService.GetSchedule()))
                Schedule.Add(subject);
        }
    }
}
