using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Reactive.Subjects;
using static AvaloniaTodoAPp.App;

namespace AvaloniaTodoAPp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    static readonly Random Rnd = new();

    public MainWindowViewModel()
    {
        var randomTasks = Application.Current?.Resources["RandomTasks"] as List<string>;
        Watermark = randomTasks![Rnd.Next(randomTasks.Count)];

        _allTasks = Tasks =
        [
            new TodoTaskViewModel(new TodoTask(1, "Buy cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(3)))),
            new TodoTaskViewModel(new TodoTask(2, "Accept cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                true)),
            new TodoTaskViewModel(new TodoTask(3, "Delete cookies", null, DateTime.Now.Subtract(TimeSpan.FromDays(1)))),
            new TodoTaskViewModel(new TodoTask(3, "Finish the code", null,
                DateTime.Now.Subtract(TimeSpan.FromHours(18)))),
            new TodoTaskViewModel(new TodoTask(3, "Add event handling", null,
                DateTime.Now.Subtract(TimeSpan.FromHours(9)), true)),
            new TodoTaskViewModel(new TodoTask(3, "Commit the changes", null,
                DateTime.Now.Subtract(TimeSpan.FromHours(3)))),
            new TodoTaskViewModel(new TodoTask(3, "Push the changes", null,
                DateTime.Now.Subtract(TimeSpan.FromMinutes(53))))
        ];

        WeakReferenceMessenger.Default.Register<RemoveTodoTaskMessage>(this,
            (_, message) =>
            {
                _allTasks = _allTasks.Except([message.Value]);
                UpdateTasks();
            });
        WeakReferenceMessenger.Default.Register<CompleteTodoTaskMessage>(this,
            (_, _) => { UpdateTasks(); });
    }

    private IEnumerable<TodoTaskViewModel> _allTasks;

    [ObservableProperty]
    private IEnumerable<TodoTaskViewModel> _tasks;

    [ObservableProperty]
    private string _watermark;

    [ObservableProperty]
    private string _newTaskText = string.Empty;

    [ObservableProperty]
    private int _selectedTab = 0;

    [RelayCommand]
    private void AddTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskText))
        {
            return;
        }

        _allTasks = _allTasks.Prepend(new TodoTaskViewModel(new TodoTask(3, NewTaskText, null, DateTime.Now)));
        UpdateTasks();
        NewTaskText = string.Empty;
    }

    partial void OnSelectedTabChanged(int value)
    {
        UpdateTasks();
    }

    private void UpdateTasks()
    {
        if (SelectedTab == 1)
        {
            Tasks = _allTasks.Where(vm => !vm.Completed);
            return;
        }

        if (SelectedTab == 2)
        {
            Tasks = _allTasks.Where(vm => vm.Important);
            return;
        }

        Tasks = _allTasks;
    }
}