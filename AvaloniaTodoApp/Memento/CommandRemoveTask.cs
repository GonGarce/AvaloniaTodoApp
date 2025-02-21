using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandRemoveTask(TodoTaskViewModel task, BehaviorSubject<MainWindowViewModel.TaskListChange> tasks) : MCommand
{
    public TodoTaskViewModel Task { get; private set; } = task;
    public BehaviorSubject<MainWindowViewModel.TaskListChange> Tasks { get; private set; } = tasks;
    
    public void doCommand()
    {
        Tasks.OnNext(Tasks.Value.With(Tasks.Value.Tasks.Except([Task])));
    }

    public void undoCommand()
    {
        Tasks.OnNext(Tasks.Value.With(Tasks.Value.Tasks.Append(Task)));
    }
}