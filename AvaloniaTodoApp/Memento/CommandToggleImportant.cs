using System.Collections.Generic;
using System.Linq;
using AvaloniaTodoAPp.Models;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandToggleImportant(TodoTaskViewModel task) : IMCommand
{
    private TodoTaskViewModel Task { get; } = task;
    public List<TodoTaskViewModel> DoCommand(List<TodoTaskViewModel> list)
    {
        return list
            .Select(model =>
            {
                if (Task == model) model.Important = !model.Important;
                return model;
            })
            .ToList();
    }

    public List<TodoTaskViewModel> UndoCommand(List<TodoTaskViewModel> list)
    {
        return DoCommand(list);
    }
}