using System.Collections.Generic;
using System.Linq;
using AvaloniaTodoAPp.Models;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandToggleDone(TodoTaskViewModel task) : MCommand
{
    public TodoTaskViewModel Task { get; private set; } = task;
    
    public void doCommand()
    {
        Task.Completed = !Task.Completed;
    }

    public void undoCommand()
    {
        Task.Completed = !Task.Completed;
    }
}