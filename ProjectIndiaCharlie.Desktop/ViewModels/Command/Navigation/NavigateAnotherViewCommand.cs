﻿using ProjectIndiaCharlie.Desktop.ViewModels.Command;
using ProjectIndiaCharlie.Desktop.ViewModels.Service;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Command.Navigation
{
    public class NavigateAnotherViewCommand : CommandBase
    {
        public override void Execute(object? parameter) => NavigationService.IndexNavigate(new AnotherViewModel());
    }
}