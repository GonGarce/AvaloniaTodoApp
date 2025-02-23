using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AsyncAwaitBestPractices;
using AvaloniaTodoApp.App;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using AvaloniaTodoAPp.ViewModels;
using Supabase.Postgrest;

namespace AvaloniaTodoAPp.Memento;

public class CommandClear() : IMCommand
{
    private List<TodoTaskViewModel> TasksViewModel { get; set; } = [];

    public void DoCommand()
    {
        TasksViewModel = MainWindowState.Instance().Tasks.Where(task => task.Completed).ToList();
        TasksViewModel.ForEach(task => MainWindowState.Instance().Subject.OnNext(new RemoveTask(task.Id)));

        var ids = TasksViewModel.Select(task => task.Id).ToList();
        AppState.Instance.Supabase.From<STask>()
            .Filter(task => task.Id, Constants.Operator.In, ids)
            .Delete()
            .SafeFireAndForget(OnError);
    }

    public void UndoCommand()
    {
        if (TasksViewModel.Count == 0) return;
        TasksViewModel.ForEach(task => MainWindowState.Instance().Subject.OnNext(new AddTask(task)));

        var tasks = TasksViewModel.Select(Mapper.ToModel).ToArray();
        AppState.Instance.Supabase.From<STask>()
            .Insert(tasks)
            .SafeFireAndForget(OnErrorRemove);
    }

    private void OnError(Exception exception)
    {
        Debugger.Log(5, "DB", $"Error deleting tasks {exception.Message}");
        TasksViewModel.ForEach(task => MainWindowState.Instance().Subject.OnNext(new AddTask(task)));
    }

    private void OnErrorRemove(Exception exception)
    {
        Debugger.Log(5, "DB", $"Error adding tasks {exception.Message}");
        TasksViewModel.ForEach(task => MainWindowState.Instance().Subject.OnNext(new RemoveTask(task.Id)));
    }
}