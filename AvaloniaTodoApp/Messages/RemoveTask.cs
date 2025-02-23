using System;

namespace AvaloniaTodoAPp.Messages;

public class RemoveTask(Guid id) : IUiMessage
{
    public Guid Id { get; } = id;
}