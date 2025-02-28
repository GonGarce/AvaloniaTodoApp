using System;
using System.Diagnostics;
using AsyncAwaitBestPractices;
using AvaloniaTodoApp;
using AvaloniaTodoApp.Global;
using AvaloniaTodoApp.Messages;
using AvaloniaTodoApp.Models;
using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Memento;

public abstract class CommandAddOrRemoveTask(TodoTaskViewModel task) : IMCommand
{
    private TodoTaskViewModel Task { get; } = task;

    public void AddCommand()
    {
        MainWindowState.Instance().Subject.OnNext(new AddTask(Task));

        var newTask = Mapper.ToModel(Task);
        AppState.Instance.Supabase.From<STask>().Insert(newTask)
            .ContinueWith(task =>
            {
                Debugger.Log(5, "DB", $"Task inserted {task.Result.Model!.Id} ");
                Task.Id = task.Result.Model!.Id;
                return task.Result;
            })
            .SafeFireAndForget(OnErrorAdd);
    }

    public void RemoveCommand()
    {
        MainWindowState.Instance().Subject.OnNext(new RemoveTask(Task.Id));

        AppState.Instance.Supabase.From<STask>().Where(x => x.Id == Task.Id).Delete()
            .ContinueWith(_ => { Debugger.Log(5, "DB", $"Deleted task {Task.Id}"); })
            .SafeFireAndForget(OnErrorRemove);
    }

    private void OnErrorAdd(Exception exception)
    {
        Debugger.Log(5, "DB", $"Error inserting task {exception.Message}");
        MainWindowState.Instance().Subject.OnNext(new RemoveTask(Task.Id));
    }

    private void OnErrorRemove(Exception exception)
    {
        Debugger.Log(5, "DB", $"Error deleting task {exception.Message}");
        MainWindowState.Instance().Subject.OnNext(new AddTask(Task));
    }

    public abstract void DoCommand();
    public abstract void UndoCommand();
}