using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands
{
    public class RequestRevisionAsyncCommand : AsyncCommandBase
    {
        public async override Task ExecuteAsync(object? parameter)
        {
            var revisionViewModel = parameter as RequestRevisionViewModel;

            await StudentService.RequestGradeRevision(revisionViewModel!.SelectedSubject.SubjectDetailId);

            _ = Task.Run(() => MessageBox.Show($"Successfully requested grade revision for {revisionViewModel.SelectedSubject.Section}."));

            revisionViewModel.SelectedSubjects.Remove(revisionViewModel.SelectedSubject);
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter is not RequestRevisionViewModel revisionViewModel)
                return false;

            if (revisionViewModel.SelectedSubject is null)
                return false;

            return true;
        }
    }
}
