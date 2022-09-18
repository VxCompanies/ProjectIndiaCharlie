using System;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Stores;

public static class NavigationStore
{
    // Main
    private static ViewModelBase? currentMainViewModel;
    public static ViewModelBase? CurrentMainViewModel
    {
        get => currentMainViewModel;
        set
        {
            currentMainViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public static event Action? CurrentMainViewModelChanged;

    public static void OnCurrentViewModelChanged() => CurrentMainViewModelChanged?.Invoke();

    // Index
    private static ViewModelBase? currentIndexViewModel;
    public static ViewModelBase? CurrentIndexViewModel
    {
        get => currentIndexViewModel;
        set
        {
            currentIndexViewModel = value;
            OnCurrentIndexViewModelChanged();
        }
    }

    public static event Action? CurrentIndexViewModelChanged;

    public static void OnCurrentIndexViewModelChanged() => CurrentIndexViewModelChanged?.Invoke();
}
