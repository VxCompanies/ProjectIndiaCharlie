using ProjectIndiaCharlie.Desktop.ViewModels.Services;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.Navigation
{
    public class NavigateReportsCommand : CommandBase
    {
        public override void Execute(object? parameter) => NavigationService.IndexNavigate(new ReportsViewModel());
    }
}
