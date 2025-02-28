using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvaloniaTodoApp.App;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using AvaloniaTodoAPp.ViewModels;
using AvaloniaTodoApp.ViewModels.Collections;
using AvaloniaTodoAPp.ViewModels.Collections;
using CommunityToolkit.Mvvm.Messaging;
using Supabase.Postgrest;
using Supabase.Postgrest.Responses;

namespace AvaloniaTodoAPp;

public class MainWindowState
{
    private MainWindowState()
    {
    }

    private static MainWindowState? _instance;
    public IEnumerable<TodoTaskViewModel> Tasks { get; private set; } = [];

    //TODO: Remember last selected tab
    public CollectionItemViewModel CurrentCollection { get; set; } = CollectionsTabViewModel.AppCollections.First();

    public readonly Subject<IUiMessage> Subject = new();

    public static MainWindowState Instance()
    {
        if (_instance != null)
        {
            return _instance;
        }

        _instance = new MainWindowState();

        _instance.Subject.Subscribe(_instance.OnMessage);
        AppState.Instance.Supabase.From<STask>()
            .Where(task => task.CompletedAt == null) // Start loading all  uncompleted tasks
            .Get().ContinueWith(task =>
            {
                var list = task.Result.Models.Select(t => new TodoTaskViewModel(t)).ToList();
                _instance.Tasks = list;
                Dispatcher.UIThread.Post(() => WeakReferenceMessenger.Default.Send(new TaskListUpdate(list)));
            });

        return _instance;
    }

    private void OnMessage(IUiMessage message)
    {
        {
            switch (message)
            {
                case AddTask addTask:
                    Tasks = Tasks.Append(addTask.Task);
                    break;
                case RemoveTask removeTask:
                    Tasks = Tasks.Where(task => !task.Id.Equals(removeTask.Id));
                    break;
                case UpdateOrAddTask updateTask:
                    Tasks = Tasks.Where(task => !task.Id.Equals(updateTask.Task.Id)).Append(updateTask.Task);
                    break;
            }

            Dispatcher.UIThread.Post(() => WeakReferenceMessenger.Default.Send(new TaskListUpdate(Tasks.ToList())));
        }
    }

    public void ChangeCollection(CollectionItemViewModel collection)
    {
        if (CurrentCollection.Id == collection.Id) return;

        if (collection.IsTodo)
        {
            LoadTodo();
        }
        else if (collection.IsCritical)
        {
            LoadCritical();
        }
        else
        {
            LoadCollection(collection);
        }

        CurrentCollection = collection;
        WeakReferenceMessenger.Default.Send(new ChangeCurrentCollection(collection));
    }

    private void LoadCritical()
    {
        AppState.Instance.Supabase.From<STask>()
            .Where(task => task.Critical == true)
            .Get().ContinueWith(UpdateTasks);
    }

    private void LoadTodo()
    {
        AppState.Instance.Supabase.From<STask>()
            .Where(task => task.CompletedAt == null)
            .Get().ContinueWith(UpdateTasks);
    }

    private void LoadCollection(CollectionItemViewModel collection)
    {
        AppState.Instance.Supabase.From<STask>()
            .Filter("collections.id", Constants.Operator.Equals, collection.Id)
            .Get().ContinueWith(UpdateTasks);
    }

    private void UpdateTasks(Task<ModeledResponse<STask>> task)
    {
        var list = task.Result.Models.Select(t => new TodoTaskViewModel(t)).ToList();
        Tasks = list;
        Dispatcher.UIThread.Post(() => WeakReferenceMessenger.Default.Send(new TaskListUpdate(list)));
    }
}