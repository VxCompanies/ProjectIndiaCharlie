using ProjectIndiaCharlie.Desktop.ViewModels.Stores;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Services;

public static class NavigationService
{
    /// <summary>
    /// Changes the main navigation current viewmodel.
    /// </summary>
    /// <typeparam name="TMainViewModel">Initializes the new main navigation viewmodel.</typeparam>
    /// <param name="newViewModel"></param>
    public static void MainNavigate<TMainViewModel>(TMainViewModel mainViewModel) where TMainViewModel : ViewModelBase => NavigationStore.CurrentMainViewModel = mainViewModel;

    /// <summary>
    /// Navigates the index navigation viewmodel.
    /// </summary>
    /// <typeparam name="TIndexViewModel">Initializes the new index navigation viewmodel.</typeparam>
    /// <param name="newIndexViewModel"></param>
    public static void IndexNavigate<TIndexViewModel>(TIndexViewModel indexViewModel) where TIndexViewModel : ViewModelBase => NavigationStore.CurrentIndexViewModel = indexViewModel;
}
