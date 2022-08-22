using ProjectIndiaCharlie.Desktop.ViewModel.Service;

namespace ProjectIndiaCharlie.Desktop.ViewModel.Command.Navigation
{
    public class NavigateHomeCommand : CommandBase
    {
        public override void Execute(object? parameter) => NavigationService.IndexNavigate(new HomeViewModel());
    }
}
