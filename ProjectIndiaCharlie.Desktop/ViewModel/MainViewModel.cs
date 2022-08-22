using ProjectIndiaCharlie.Desktop.ViewModel.Store;

namespace ProjectIndiaCharlie.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentMainViewModel => NavigationStore.CurrentMainViewModel;

        public MainViewModel() => NavigationStore.CurrentMainViewModelChanged += OnCurrentMainViewModelChanged;

        private void OnCurrentMainViewModelChanged() => OnPropertyChanged(nameof(CurrentMainViewModel));
    }
}
