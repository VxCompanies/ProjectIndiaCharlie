using ProjectIndiaCharlie.Desktop.ViewModels.Services;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.Navigation;

public class NavigateScheduleCommand : CommandBase
{
    public override void Execute(object? parameter) => NavigationService.IndexNavigate(new ScheduleViewModel());
}
