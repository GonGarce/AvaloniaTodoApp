using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AvaloniaTodoAPp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTodoAPp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private IEnumerable<TodoTaskViewModel> tasks;

    public MainWindowViewModel()
    {
        tasks =
        [
            new TodoTaskViewModel(new TodoTask(1, "Buy cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(3)))),
            new TodoTaskViewModel(new TodoTask(2, "Accept cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(2)))),
            new TodoTaskViewModel(new TodoTask(3, "Delete cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(1))))
        ];
    }
}