using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Command;

public abstract class AsyncCommandBase : CommandBase
{
    public bool IsExecuting { get; private set; }

    public override async void Execute(object? parameter)
    {
        IsExecuting = true;

        try
        {
            await ExecuteAsync(parameter);
        }
        finally
        {
            IsExecuting = false;
        }
    }

    public override bool CanExecute(object? parameter) => !IsExecuting && base.CanExecute(parameter);

    public abstract Task ExecuteAsync(object? parameter);
}
