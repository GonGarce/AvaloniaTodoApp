using System.Collections.Generic;

namespace AvaloniaTodoAPp.Memento;

public class Memento
{
    private const int Memory = 20;
    private int _actions;
    private List<MCommand> _history = new();

    public int Undo()
    {
        if (_actions == 0) return 0;
        _actions--;
        var command = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        command.undoCommand();
        return _actions;
    }

    public int DoCommand(MCommand command)
    {
        _history.Add(command);
        command.doCommand();
        _actions += _actions == Memory ? 0 : 1;
        
        if (_history.Count == Memory * 2)
        {
            _history = _history.Slice(Memory, Memory);
        }
        
        return _actions;
    }
}