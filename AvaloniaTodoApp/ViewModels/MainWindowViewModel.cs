using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace AvaloniaTodoAPp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private IEnumerable<TodoTaskViewModel> _tasks;

    public MainWindowViewModel()
    {
        Tasks =
        [
            new TodoTaskViewModel(new TodoTask(1, "Buy cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(3)))),
            new TodoTaskViewModel(new TodoTask(2, "Accept cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(2)), true)),
            new TodoTaskViewModel(new TodoTask(3, "Delete cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(1)))),
            new TodoTaskViewModel(new TodoTask(3, "Finish the code", null, DateTime.Now.Subtract(TimeSpan.FromHours(18)))),
            new TodoTaskViewModel(new TodoTask(3, "Add event handling", null, DateTime.Now.Subtract(TimeSpan.FromHours(9)), true)),
            new TodoTaskViewModel(new TodoTask(3, "Commit the changes", null, DateTime.Now.Subtract(TimeSpan.FromHours(3)))),
            new TodoTaskViewModel(new TodoTask(3, "Push the changes", null, DateTime.Now.Subtract(TimeSpan.FromMinutes(53)))),
            new TodoTaskViewModel(new TodoTask(1, "Buy cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(3)))),
            new TodoTaskViewModel(new TodoTask(2, "Accept cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(2)), true)),
            new TodoTaskViewModel(new TodoTask(3, "Delete cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(1)))),
            new TodoTaskViewModel(new TodoTask(3, "Finish the code", null, DateTime.Now.Subtract(TimeSpan.FromHours(18)))),
            new TodoTaskViewModel(new TodoTask(3, "Add event handling", null, DateTime.Now.Subtract(TimeSpan.FromHours(9)), true)),
            new TodoTaskViewModel(new TodoTask(3, "Commit the changes", null, DateTime.Now.Subtract(TimeSpan.FromHours(3)))),
            new TodoTaskViewModel(new TodoTask(3, "Push the changes", null, DateTime.Now.Subtract(TimeSpan.FromMinutes(53))))
        ];

        WeakReferenceMessenger.Default.Register<RemoveTodoTaskMessage>(this, (r, message) =>
        {
            Tasks = Tasks.Except([message.Value]);
        });
    }
}