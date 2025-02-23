using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandRemoveTask(TodoTaskViewModel task) : CommandAddOrRemoveTask(task)
{

    public override void DoCommand()
    {
        RemoveCommand();
    }

    public override void UndoCommand()
    {
        AddCommand();
    }
}