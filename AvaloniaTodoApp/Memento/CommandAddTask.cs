using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandAddTask(TodoTaskViewModel task, BehaviorSubject<MainWindowViewModel.TaskListChange> subject) : MCommand
{
    private TodoTaskViewModel Task { get; } = task;
    private BehaviorSubject<MainWindowViewModel.TaskListChange> Subject { get; } = subject;
    
    public void doCommand()
    {
        Subject.OnNext(Subject.Value.With(Subject.Value.Tasks.Prepend(Task)));
    }

    public void undoCommand()
    {
        Subject.OnNext(Subject.Value.With(Subject.Value.Tasks.Skip(1)));
    }
}