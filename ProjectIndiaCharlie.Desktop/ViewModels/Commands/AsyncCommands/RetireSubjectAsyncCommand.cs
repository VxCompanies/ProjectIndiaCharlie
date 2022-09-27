using System;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands
{
    public class RetireSubjectAsyncCommand : AsyncCommandBase
    {
        public override Task ExecuteAsync(object? parameter)
        {
            throw new NotImplementedException();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
    }
}
