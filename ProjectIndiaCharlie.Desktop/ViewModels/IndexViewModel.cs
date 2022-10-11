using ProjectIndiaCharlie.Desktop.ViewModels.Commands;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands.Navigation;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public ViewModelBase? CurrentIndexViewModel => NavigationStore.CurrentIndexViewModel;

        public NavigateHomeCommand NavigateHomeCommand { get; }
        public NavigateSelectionCommand NavigateSelectionCommand { get; }
        public NavigateRetirementCommand NavigateRetirementCommand { get; }
        public NavigateRequestRevisionCommand NavigateRequestRevisionCommand { get; }
        public LogoutCommand LogoutCommand { get; }

        public IndexViewModel()
        {
            NavigationStore.CurrentIndexViewModelChanged += OnCurrentIndexViewModelChanged;

            NavigateHomeCommand = new();
            NavigateSelectionCommand = new();
            NavigateRetirementCommand = new();
            NavigateRequestRevisionCommand = new();
            LogoutCommand = new();
        }

        private void OnCurrentIndexViewModelChanged() => OnPropertyChanged(nameof(CurrentIndexViewModel));
    }
}
