using System.Collections.Generic;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public interface IMCommand
{
    public void DoCommand();
    public void UndoCommand();
}