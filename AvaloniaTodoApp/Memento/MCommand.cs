using System.Collections.Generic;
using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Memento;

public interface IMCommand
{
    public void DoCommand();
    public void UndoCommand();
}