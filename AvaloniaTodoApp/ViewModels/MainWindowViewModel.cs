using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using AvaloniaTodoAPp.Memento;

namespace AvaloniaTodoAPp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private static readonly Random Rnd = new();

    public MainWindowViewModel()
    {
        var randomTasks = Application.Current?.Resources["RandomTasks"] as List<string>;
        Watermark = randomTasks![Rnd.Next(randomTasks.Count)];

        Tasks =
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
            (_, message) => { DoCommand(new CommandRemoveTask(message.Value)); });
        WeakReferenceMessenger.Default.Register<ChangedTodoTaskMessage>(this,
            (_, message) => { DoCommand(message.Value); });
    }

    private readonly Memento.Memento _memento = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowedTasks))]
    private List<TodoTaskViewModel> _tasks;

    public IEnumerable<TodoTaskViewModel> ShowedTasks => FilterTasks();

    [ObservableProperty]
    private string _watermark;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowedTasks))]
    [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
    private string _newTaskText = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowedTasks))]
    private int _selectedTab;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowedTasks))]
    private bool _sortRecentFirst = true;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UndoCommand))]
    private int _undoCount;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClearTasksCommand))]
    private bool _hasTodoTasks;

    partial void OnTasksChanged(List<TodoTaskViewModel> value)
    {
        HasTodoTasks = value.Any(model => model.Completed);
    }

    private bool CanAddTask() => !string.IsNullOrWhiteSpace(NewTaskText);

    [RelayCommand(CanExecute = nameof(CanAddTask))]
    private void AddTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskText)) return;
        DoCommand(new CommandAddTask(new TodoTaskViewModel(new TodoTask(3, NewTaskText, null, DateTime.Now))));
        NewTaskText = string.Empty;
    }

    private bool CanClearTasks() => HasTodoTasks;

    [RelayCommand(CanExecute = nameof(CanClearTasks))]
    private void ClearTasks()
    {
        IMCommand command = new CommandList(Tasks
            .Where(vm => vm.Completed)
            .Select(model => new CommandRemoveTask(model))
            .Cast<IMCommand>().ToList());
        DoCommand(command);
    }

    [RelayCommand]
    private void SortTasks(bool recentFirst)
    {
        SortRecentFirst = recentFirst;
    }

    private bool CanUndo() => UndoCount > 0;

    [RelayCommand(CanExecute = nameof(CanUndo))]
    private void Undo()
    {
        Tasks = _memento.Undo(Tasks);
        UndoCount = _memento.GetUndoCount();
    }

    private void DoCommand(IMCommand command)
    {
        Tasks = _memento.DoCommand(command, Tasks);
        UndoCount = _memento.GetUndoCount();
    }

    private IEnumerable<TodoTaskViewModel> FilterTasks()
    {
        IEnumerable<TodoTaskViewModel> tasks = Tasks;
        tasks = SelectedTab switch
        {
            1 => tasks.Where(vm => !vm.Completed),
            2 => tasks.Where(vm => vm.Important),
            _ => tasks
        };

        tasks = SortRecentFirst
            ? tasks.OrderByDescending(model => model.CreationDate)
            : tasks.OrderBy(model => model.CreationDate);

        if (!string.IsNullOrWhiteSpace(NewTaskText))
        {
            tasks = tasks.Where(model =>
                model.Description.Contains(NewTaskText, StringComparison.CurrentCultureIgnoreCase));
        }

        return tasks;
    }
}