using ProjectIndiaCharlie.Desktop.ViewModel.Command.Navigation;
using ProjectIndiaCharlie.Desktop.ViewModel.Store;

namespace ProjectIndiaCharlie.Desktop.ViewModel
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
