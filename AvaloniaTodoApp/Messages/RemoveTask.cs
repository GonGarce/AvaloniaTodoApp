using System;

namespace AvaloniaTodoApp.Messages;

public class RemoveTask(Guid id) : IUiMessage
{
    public Guid Id { get; } = id;
}