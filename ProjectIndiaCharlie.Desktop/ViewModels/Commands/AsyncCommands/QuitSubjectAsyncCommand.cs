using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands
{
    public class QuitSubjectAsyncCommand : AsyncCommandBase
    {
        public override async Task ExecuteAsync(object? parameter)
        {
            var selectionViewModel = parameter as SelectionViewModel;

            var response = await StudentService.SubjectElimination(selectionViewModel!.SelectedScheduleSubject.SubjectDetailId);

            selectionViewModel.SelectedSubjects.Remove(selectionViewModel.SelectedScheduleSubject);

            MessageBox.Show(response);
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter is not SelectionViewModel selectionViewModel)
                return false;

            if (selectionViewModel.SelectedScheduleSubject is null)
                return false;

            return true;
        }
    }
}
