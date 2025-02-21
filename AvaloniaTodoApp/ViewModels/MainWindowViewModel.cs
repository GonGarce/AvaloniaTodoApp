using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Reactive.Subjects;
using AvaloniaTodoAPp.Memento;

namespace AvaloniaTodoAPp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private static readonly Random Rnd = new();

    public MainWindowViewModel()
    {
        var randomTasks = Application.Current?.Resources["RandomTasks"] as List<string>;
        Watermark = randomTasks![Rnd.Next(randomTasks.Count)];

        _taskChangeSubject = new BehaviorSubject<TaskListChange>(new TaskListChange(0, [], true));
        FilteredTasks = _taskChangeSubject.Select(FilterTasks);
        DoneTaskCount = _taskChangeSubject
            .Select(change => change.Tasks)
            .Select(tasks => tasks.Count(task => task.Completed));

        _taskChangeSubject.OnNext(_taskChangeSubject.Value.With([
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
        ]));

        WeakReferenceMessenger.Default.Register<RemoveTodoTaskMessage>(this,
            (_, message) =>
            {
                MCommand command = new CommandRemoveTask(message.Value, _taskChangeSubject);
                UndoCount = _memento.DoCommand(command);
            });
        WeakReferenceMessenger.Default.Register<ChangedTodoTaskMessage>(this,
            (_, message) =>
            {
                UndoCount = _memento.DoCommand(message.Value);
                _taskChangeSubject.OnNext(_taskChangeSubject.Value); // Update needed for filtered lists
            });
    }

    private readonly Memento.Memento _memento = new();
    
    private readonly BehaviorSubject<TaskListChange> _taskChangeSubject;
    public IObservable<IEnumerable<TodoTaskViewModel>> FilteredTasks { get; private set; }
    public IObservable<int> DoneTaskCount { get; private set; }

    [ObservableProperty]
    private string _watermark;

    [ObservableProperty]
    private string _newTaskText = string.Empty;

    [ObservableProperty]
    private int _selectedTab;

    [ObservableProperty]
    private bool _sortRecentFirst = true;

    [ObservableProperty]
    private int _undoCount;

    [RelayCommand]
    private void AddTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskText))
        {
            return;
        }

        MCommand command = new CommandAddTask(
            new TodoTaskViewModel(new TodoTask(3, NewTaskText, null, DateTime.Now)),
            _taskChangeSubject);
        UndoCount = _memento.DoCommand(command);
        NewTaskText = string.Empty;
    }

    [RelayCommand]
    private void ClearTasks()
    {
        MCommand command = new CommandList(_taskChangeSubject.Value.Tasks
            .Where(vm => vm.Completed)
            .Select(model => new CommandRemoveTask(model, _taskChangeSubject))
            .Cast<MCommand>().ToList());
        UndoCount = _memento.DoCommand(command);
    }

    [RelayCommand]
    private void SortTasks(bool recentFirst)
    {
        SortRecentFirst = recentFirst;
        _taskChangeSubject.OnNext(_taskChangeSubject.Value.With(recentFirst));
    }

    [RelayCommand]
    private void Undo()
    {
        UndoCount = _memento.Undo();
    }

    partial void OnSelectedTabChanged(int value)
    {
        _taskChangeSubject.OnNext(_taskChangeSubject.Value.With(value));
    }

    private IEnumerable<TodoTaskViewModel> FilterTasks(TaskListChange change)
    {
        var tasks = change.Tasks;
        tasks = change.SelectedTab switch
        {
            1 => tasks.Where(vm => !vm.Completed),
            2 => tasks.Where(vm => vm.Important),
            _ => tasks
        };

        tasks = change.RecentFirst
            ? tasks.OrderByDescending(model => model.CreationDate)
            : tasks.OrderBy(model => model.CreationDate);

        return tasks;
    }

    public class TaskListChange(int selectedTab, IEnumerable<TodoTaskViewModel> tasks, bool recentFirst)
    {
        public int SelectedTab { get; } = selectedTab;
        public IEnumerable<TodoTaskViewModel> Tasks { get; } = tasks;

        public bool RecentFirst { get; } = recentFirst;

        public TaskListChange With(int newTab)
        {
            return new TaskListChange(newTab, Tasks, RecentFirst);
        }

        public TaskListChange With(IEnumerable<TodoTaskViewModel> tasks)
        {
            return new TaskListChange(SelectedTab, tasks, RecentFirst);
        }

        public TaskListChange With(bool recentFirst)
        {
            return new TaskListChange(SelectedTab, Tasks, recentFirst);
        }
    }
}