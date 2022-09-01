using ProjectIndiaCharlie.Desktop.ViewModels.Command.Navigation;
using ProjectIndiaCharlie.Desktop.ViewModels.Store;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public ViewModelBase? CurrentIndexViewModel => NavigationStore.CurrentIndexViewModel;

        public NavigateHomeCommand NavigateHomeCommand { get; }
        public NavigateAnotherViewCommand NavigateAnotherViewCommand { get; }

        public IndexViewModel()
        {
            NavigationStore.CurrentIndexViewModelChanged += OnCurrentIndexViewModelChanged;

            NavigateHomeCommand = new NavigateHomeCommand();
            NavigateAnotherViewCommand = new NavigateAnotherViewCommand();
        }

        private void OnCurrentIndexViewModelChanged() => OnPropertyChanged(nameof(CurrentIndexViewModel));
    }
}
