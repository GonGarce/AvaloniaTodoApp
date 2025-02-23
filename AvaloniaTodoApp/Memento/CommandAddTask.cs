using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandAddTask(TodoTaskViewModel task) : CommandAddOrRemoveTask(task)
{
    public override void DoCommand()
    {
        AddCommand();
    }

    public override void UndoCommand()
    {
        RemoveCommand();
    }
}