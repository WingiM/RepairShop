using System;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;
using MaterialDesignThemes.Wpf;

namespace RepairShop.ViewModels.Base;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string _viewModelTitle = "Ремонтная мастерская";

    [ObservableProperty]
    private SnackbarMessageQueue _snackbarMessageQueue = null!;

    protected BaseViewModel()
    {
        SnackbarMessageQueue = new SnackbarMessageQueue();
    }

    public virtual NavigationResult OnNavigatedTo(NavigationArgs args)
    {
        return new NavigationResult { IsSuccess = true, NavigationArgs = args };
    }

    public void PushErrorToSnackbar(Exception ex)
    {
        var message = ex is ValidationException vex
            ? string.Join('\n', vex.Errors.Select(z => z.ErrorMessage))
            : ex.Message;
        WriteToSnackBar(message);
    }

    public void WriteToSnackBar(string message)
    {
        SnackbarMessageQueue.Enqueue(message);
    }
}
