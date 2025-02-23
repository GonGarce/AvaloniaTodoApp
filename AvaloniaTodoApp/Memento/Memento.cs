using System.Collections.Generic;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class Memento
{
    private const int Memory = 20;
    private int _actions;
    private List<IMCommand> _history = new(20);

    public void Undo()
    {
        if (_actions == 0) return;
        _actions--;
        var command = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        command.UndoCommand();
    }

    public void DoCommand(IMCommand command)
    {
        _history.Add(command);
        _actions += _actions == Memory ? 0 : 1;

        if (_history.Count == Memory * 2)
        {
            _history = _history.Slice(Memory, Memory);
        }

        command.DoCommand();
    }

    public int GetUndoCount()
    {
        return _actions;
    }

    public void clear()
    {
        _history.Clear();
        _actions = 0;
    }
}