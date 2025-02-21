using System.Collections.Generic;
using System.Linq;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandRemoveTask(TodoTaskViewModel task) : IMCommand
{
    private TodoTaskViewModel Task { get; } = task;

    public List<TodoTaskViewModel> DoCommand(List<TodoTaskViewModel> list)
    {
        return list.Except([Task]).ToList();
    }

    public List<TodoTaskViewModel> UndoCommand(List<TodoTaskViewModel> list)
    {
        return list.Append(Task).ToList();
    }
}