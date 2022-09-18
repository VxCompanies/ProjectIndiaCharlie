using ProjectIndiaCharlie.Desktop.ViewModels.Commands.Navigation;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public ViewModelBase? CurrentIndexViewModel => NavigationStore.CurrentIndexViewModel;

        public NavigateHomeCommand NavigateHomeCommand { get; }

        public IndexViewModel()
        {
            NavigationStore.CurrentIndexViewModelChanged += OnCurrentIndexViewModelChanged;

            NavigateHomeCommand = new NavigateHomeCommand();
        }

        private void OnCurrentIndexViewModelChanged() => OnPropertyChanged(nameof(CurrentIndexViewModel));
    }
}
