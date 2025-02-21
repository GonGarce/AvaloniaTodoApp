using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandAddTask(TodoTaskViewModel task, BehaviorSubject<MainWindowViewModel.TaskListChange> subject) : MCommand
{
    public TodoTaskViewModel Task { get; private set; } = task;
    public BehaviorSubject<MainWindowViewModel.TaskListChange> Subject { get; private set; } = subject;
    
    public void doCommand()
    {
        Subject.OnNext(Subject.Value.With(Subject.Value.Tasks.Prepend(task)));
    }

    public void undoCommand()
    {
        Subject.OnNext(Subject.Value.With(Subject.Value.Tasks.Skip(1)));
    }
}