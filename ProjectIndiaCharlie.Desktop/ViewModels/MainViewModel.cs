using ProjectIndiaCharlie.Desktop.ViewModels.Store;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentMainViewModel => NavigationStore.CurrentMainViewModel;

        public MainViewModel() => NavigationStore.CurrentMainViewModelChanged += OnCurrentMainViewModelChanged;

        private void OnCurrentMainViewModelChanged() => OnPropertyChanged(nameof(CurrentMainViewModel));
    }
}
