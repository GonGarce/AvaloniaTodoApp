using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Memento;

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