using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using AvaloniaTodoApp.App;
using AvaloniaTodoAPp.Controls;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using AvaloniaTodoAPp.Memento;
using AvaloniaTodoApp.ViewModels.Collections;
using AvaloniaTodoAPp.ViewModels.Collections;
using Supabase.Realtime;
using Supabase.Realtime.PostgresChanges;

namespace AvaloniaTodoAPp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private static readonly Random Rnd = new();

    private RealtimeChannel? _channel; // TODO: Unsubscribe

    public async void LoadAvatar()
    {
        var url = AppState.Instance.Profile.Image;
        if (string.IsNullOrEmpty(url)) return;
        HttpClient s_httpClient = new();
        var data = await s_httpClient.GetByteArrayAsync(url);
        var m = new MemoryStream(data);
        var avatar = await Task.Run(() => Bitmap.DecodeToWidth(m, 36));
        Dispatcher.UIThread.Post(() => Avatar = avatar);
        //await using (var imageStream = await _album.LoadCoverBitmapAsync())
    }

    [ObservableProperty]
    private Bitmap? _avatar;

    private async void SubscribeChannel()
    {
        _channel = await AppState.Instance.Supabase.From<STask>().On(PostgresChangesOptions.ListenType.All,
            (_, change) =>
            {
                if (change.Payload?.Data?.Type is Constants.EventType.Insert or Constants.EventType.Update)
                {
                    var t = change.Model<STask>()!;

                    if (!MainWindowState.Instance().CurrentCollection.IsDefault &&
                        t.CollectionId != MainWindowState.Instance().CurrentCollection.Id)
                    {
                        // Change is not from the current collection
                        return;
                    }

                    if (SelectedCollection.IsTodo && t.IsCompleted() || SelectedCollection.IsCritical && !t.Critical)
                    {
                        // If completed or critical has changed maybe we have to hide from the list.
                        MainWindowState.Instance().Subject.OnNext(new RemoveTask(t.Id));
                    }
                    else
                    {
                        // If task must be shown, then send add message
                        MainWindowState.Instance().Subject.OnNext(new UpdateOrAddTask(new TodoTaskViewModel(t)));
                    }
                }

                if (change.Payload?.Data?.Type == Constants.EventType.Delete)
                {
                    var t = change.OldModel<STask>()!;
                    MainWindowState.Instance().Subject.OnNext(new RemoveTask(t.Id));
                }
            });
    }

    public MainWindowViewModel()
    {
        var randomTasks = Application.Current?.Resources["RandomTasks"] as List<string>;
        Watermark = randomTasks![Rnd.Next(randomTasks.Count)];

        if (Design.IsDesignMode) return;

        SubscribeChannel();

        WeakReferenceMessenger.Default.Register<RemoveTodoTaskMessage>(this,
            (_, message) => { DoCommand(new CommandRemoveTask(message.Value)); });
        WeakReferenceMessenger.Default.Register<ChangedTodoTaskMessage>(this,
            (_, message) => { DoCommand(message.Value); });

        Tasks = MainWindowState.Instance().Tasks.ToList();
        WeakReferenceMessenger.Default.Register<TaskListUpdate>(this, (_, message) => { Tasks = message.Value; });

        SelectedCollection = MainWindowState.Instance().CurrentCollection;
        WeakReferenceMessenger.Default.Register<ChangeCurrentCollection>(this,
            (_, message) => Dispatcher.UIThread.Post(() =>
            {
                SelectedCollection = message.Value;
                UndoCount = 0;
                _memento.clear();
            }));
    }

    private readonly Memento.Memento _memento = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowTabs))]
    [NotifyPropertyChangedFor(nameof(Avatars))]
    private CollectionItemViewModel _selectedCollection = null!;

    public CollectionsTabViewModel CollectionsTabViewModel { get; set; } = new();

    public bool ShowTabs => !SelectedCollection.IsDefault;

    public IEnumerable<AvatarList.AvatarData> Avatars =>
        SelectedCollection.Profiles.Select(profile => new AvatarList.AvatarData { Username = profile.Username });

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowedTasks))]
    private List<TodoTaskViewModel> _tasks = [];

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

    private bool CanAddTask()
    {
        return !string.IsNullOrWhiteSpace(NewTaskText) && SelectedCollection is { IsDefault: false };
    }

    [RelayCommand(CanExecute = nameof(CanAddTask))]
    private void AddTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskText)) return;
        DoCommand(new CommandAddTask(new TodoTaskViewModel(NewTaskText, SelectedCollection.Id)));
        NewTaskText = string.Empty;
    }

    private bool CanClearTasks() => HasTodoTasks;

    [RelayCommand(CanExecute = nameof(CanClearTasks))]
    private void ClearTasks()
    {
        DoCommand(new CommandClear());
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
        //Tasks = _memento.Undo(Tasks);
        _memento.Undo();
        UndoCount = _memento.GetUndoCount();
    }

    private void DoCommand(IMCommand command)
    {
        //Tasks = _memento.DoCommand(command, Tasks);
        _memento.DoCommand(command);
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