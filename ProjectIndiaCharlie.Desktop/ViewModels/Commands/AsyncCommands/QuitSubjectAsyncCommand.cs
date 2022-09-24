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

            var response = await StudentService.SubjectElimination(selectionViewModel!.SelectedSubject.SubjectDetailId);

            selectionViewModel.SelectedSubjects.Remove(selectionViewModel.SelectedSubject);

            MessageBox.Show(response);
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter is not SelectionViewModel selectionViewModel)
                return false;

            if (selectionViewModel.SelectedSubject is null)
                return false;

            return true;
        }
    }
}
