using System.Collections.Generic;
using System.Linq;
using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Memento;

public class CommandList(List<IMCommand> commands) : IMCommand
{
    private List<IMCommand> _commands = commands;

    public void DoCommand()
    {
        var commands = _commands;
        while (true)
        {
            if (commands.Count == 0) return;
            var c = commands[0];
            commands = commands.Skip(1).ToList();
        }
    }

    public void UndoCommand()
    {
        var commands = _commands;
        while (true)
        {
            if (commands.Count == 0) return;
            var c = commands[0];
            commands = commands.Skip(1).ToList();
        }
    }
}