using ProjectIndiaCharlie.Desktop.ViewModels.Commands.Navigation;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public ViewModelBase? CurrentIndexViewModel => NavigationStore.CurrentIndexViewModel;

        public NavigateHomeCommand NavigateHomeCommand { get; }
        public NavigateReportsCommand NavigateReportsCommand { get; }
        public NavigateSelectionCommand NavigateSelectionCommand { get; }

        public IndexViewModel()
        {
            NavigationStore.CurrentIndexViewModelChanged += OnCurrentIndexViewModelChanged;

            NavigateHomeCommand = new();
            NavigateReportsCommand = new();
            NavigateSelectionCommand = new();
        }

        private void OnCurrentIndexViewModelChanged() => OnPropertyChanged(nameof(CurrentIndexViewModel));
    }
}
