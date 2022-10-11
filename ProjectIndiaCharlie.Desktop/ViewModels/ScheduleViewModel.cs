using ProjectIndiaCharlie.Desktop.Models;
using System.Collections.ObjectModel;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        public ObservableCollection<VSubjectSectionDetail> Schedule { get; set; }

        public ScheduleViewModel()
        {
            Schedule = new();
            GetSchedule();
        }

        private async void GetSchedule()
        {

        }
    }
}
