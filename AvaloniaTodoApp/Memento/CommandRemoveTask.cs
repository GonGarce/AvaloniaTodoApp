using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Memento;

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