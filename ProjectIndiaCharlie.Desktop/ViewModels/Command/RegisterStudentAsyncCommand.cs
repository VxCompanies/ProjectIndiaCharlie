using ProjectIndiaCharlie.Desktop.ViewModels.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Command
{
    public class RegisterStudentAsyncCommand : AsyncCommandBase
    {
        public async override Task ExecuteAsync(object? parameter)
        {
            var homeViewModel = parameter as HomeViewModel;

            //PersonService.
        }
    }
}
