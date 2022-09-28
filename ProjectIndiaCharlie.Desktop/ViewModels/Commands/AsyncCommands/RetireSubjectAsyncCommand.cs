using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands
{
    public class RetireSubjectAsyncCommand : AsyncCommandBase
    {
        public async override Task ExecuteAsync(object? parameter)
        {
            var retirementViewModel = parameter as RetirementViewModel;

            await StudentService.SubjectRetirement(retirementViewModel!.SelectedSubject.SubjectDetailId);

            _ = Task.Run(() => MessageBox.Show($"{retirementViewModel.SelectedSubject.SubjectCode} retired successfully."));

            retirementViewModel.SelectedSubjects.Remove(retirementViewModel.SelectedSubject);
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter is not RetirementViewModel retirementViewModel)
                return false;

            if (retirementViewModel.SelectedSubject is null)
                return false;

            return true;
        }
    }
}
