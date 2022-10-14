﻿using System;
using System.Windows.Input;

namespace ProjectIndiaCharlie.Mobile.ViewModels.Commands;

public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged;
    //{
    //    add => CommandManager.RequerySuggested += value;
    //    remove => CommandManager.RequerySuggested -= value;
    //}

    public virtual bool CanExecute(object? parameter) => true;

    public abstract void Execute(object? parameter);
}
