using System.Collections.Generic;
using System.Linq;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class CommandList(List<IMCommand> commands) : IMCommand
{
    private List<IMCommand> _commands = commands;

    public List<TodoTaskViewModel> DoCommand(List<TodoTaskViewModel> list)
    {
        var commands = _commands;
        while (true)
        {
            if (commands.Count == 0) return list;
            var c = commands[0];
            commands = commands.Skip(1).ToList();
            list = c.DoCommand(list);
        }
    }

    public List<TodoTaskViewModel> UndoCommand(List<TodoTaskViewModel> list)
    {
        var commands = _commands;
        while (true)
        {
            if (commands.Count == 0) return list;
            var c = commands[0];
            commands = commands.Skip(1).ToList();
            list = c.UndoCommand(list);
        }
    }
}