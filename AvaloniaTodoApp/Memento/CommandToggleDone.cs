using System;
using System.Diagnostics;
using AsyncAwaitBestPractices;
using AvaloniaTodoApp;
using AvaloniaTodoApp.Global;
using AvaloniaTodoApp.Messages;
using AvaloniaTodoApp.Models;
using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Memento;

public class CommandToggleDone(TodoTaskViewModel task) : IMCommand
{
    private TodoTaskViewModel Task { get; } = task;
    public void DoCommand()
    {
        SetCompleted(!Task.Completed);
    }

    public void UndoCommand()
    {
        SetCompleted(!Task.Completed);
    }

    private void SetCompleted(bool completed)
    {
        Task.Completed = completed;
        MainWindowState.Instance().Subject.OnNext(new UpdateOrAddTask(Task));

        AppState.Instance.Supabase.From<STask>()
            .Where(x => x.Id == Task.Id)
            .Set(x => x.CompletedAt!, completed ? DateTime.Now : null)
            .Update()
            .ContinueWith(task =>
            {
                Debugger.Log(5, "DB", $"Task updated {task.Result.Model!.Id}");
                Task.Id = task.Result.Model!.Id;
                return task.Result;
            })
            .SafeFireAndForget(OnErrorDo);
    }
    
    private void OnErrorDo(Exception exception)
    {
        Debugger.Log(5, "DB", $"Error updating task {exception.Message}");
        Task.Completed = !Task.Completed;
        MainWindowState.Instance().Subject.OnNext(new UpdateOrAddTask(Task));
    }
}