using ProjectIndiaCharlie.Desktop.ViewModels.Services;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.Navigation
{
    public class NavigateLoginCommand : CommandBase
    {
        public override void Execute(object? parameter) => NavigationService.IndexNavigate(new LoginViewModel());
    }
}
