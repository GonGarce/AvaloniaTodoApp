using System.Collections.Generic;
using System.Linq;

namespace AvaloniaTodoAPp.Memento;

public class CommandList(List<MCommand> commands) : MCommand
{
    public List<MCommand> Commands { get; } = commands;
    
    public void doCommand()
    {
        Commands.ForEach(command => command.doCommand());
    }

    public void undoCommand()
    {
        Commands.ForEach(command => command.undoCommand());
    }
}