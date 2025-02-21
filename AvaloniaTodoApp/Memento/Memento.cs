using System.Collections.Generic;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Memento;

public class Memento
{
    private const int Memory = 20;
    private int _actions;
    private List<IMCommand> _history = new();

    public List<TodoTaskViewModel> Undo(List<TodoTaskViewModel> list)
    {
        if (_actions == 0) return list;
        _actions--;
        var command = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        return command.UndoCommand(list);
    }

    public List<TodoTaskViewModel> DoCommand(IMCommand command, List<TodoTaskViewModel> list)
    {
        _history.Add(command);
        _actions += _actions == Memory ? 0 : 1;
        
        if (_history.Count == Memory * 2)
        {
            _history = _history.Slice(Memory, Memory);
        }
        
        return command.DoCommand(list);
    }

    public int GetUndoCount()
    {
        return _actions;
    }
}