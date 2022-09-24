using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands
{
    public class LoginAsyncCommand : AsyncCommandBase
    {
        public async override Task ExecuteAsync(object? parameter)
        {
            var loginViewModel = parameter as LoginViewModel;

            if (int.TryParse(loginViewModel!.UserId, out int userId))
                return;

            if (!await StudentService.Login(loginViewModel!.UserId, loginViewModel!.Password))
            {
                MessageBox.Show("Wrong id or password.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            NavigationService.MainNavigate(new IndexViewModel());
            NavigationService.IndexNavigate(new HomeViewModel());
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter is not LoginViewModel loginViewModel)
                return false;

            if (string.IsNullOrWhiteSpace(loginViewModel.UserId))
                return false;

            if (string.IsNullOrWhiteSpace(loginViewModel.Password))
                return false;

            return base.CanExecute(parameter);
        }
    }
}
