using System.Collections.Generic;
using System.Linq;
using AvaloniaTodoAPp.Models;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandToggleImportant(TodoTaskViewModel task) : MCommand
{
    public TodoTaskViewModel Task { get; private set; } = task;
    
    public void doCommand()
    {
        Task.Important = !Task.Important;
    }

    public void undoCommand()
    {
        Task.Important = !Task.Important;
    }
}