using System.Collections.Generic;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public interface IMCommand
{
    public List<TodoTaskViewModel> DoCommand(List<TodoTaskViewModel> list);
    public List<TodoTaskViewModel> UndoCommand(List<TodoTaskViewModel> list);
}