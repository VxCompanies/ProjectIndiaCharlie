using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands
{
    public class SelectSubjectAsyncCommand : AsyncCommandBase
    {
        public override async Task ExecuteAsync(object? parameter)
        {
            var selectionViewModel = parameter as SelectionViewModel;

            var response = await StudentService.SubjectSelection(selectionViewModel!.SelectedSelectionSubject.SubjectDetailId);

            _ = Task.Run(() => MessageBox.Show(response));

            if (response.ToLower().Contains("cannot"))
                return;
            
            if (response.ToLower().Contains("section"))
                return;
            
            selectionViewModel.SelectedSubjects.Add(selectionViewModel.SelectedSelectionSubject);
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter is not SelectionViewModel selectionViewModel)
                return false;

            if (selectionViewModel.SelectedSelectionSubject is null)
                return false;

            return true;
        }
    }
}
