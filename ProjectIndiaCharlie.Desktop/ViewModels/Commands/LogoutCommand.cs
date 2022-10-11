using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands;

public class LogoutCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        LogedStudent.Student = null;
        NavigationService.MainNavigate(new LoginViewModel());
    }
}
