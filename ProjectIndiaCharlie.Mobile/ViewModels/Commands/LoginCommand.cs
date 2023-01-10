using ProjectIndiaCharlie.Mobile.ViewModels.Services;

namespace ProjectIndiaCharlie.Mobile.ViewModels.Commands;

public class LoginCommand : CommandBase
{
    public override async void Execute(object? parameter)
    {
        var loginViewModel = parameter as LoginViewModel;

        if (!await StudentService.Login(loginViewModel!.Id, loginViewModel!.Password))
        {
            //DisplayAlert("Wrong id or password.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            return;

            //Navigation
        }

        //new NavigationPage(new IndexViewModel());
        //NavigationService.IndexNavigate(new HomeViewModel());
    }
}
