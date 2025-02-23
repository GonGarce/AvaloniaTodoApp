using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Messages;

public class AddTask(TodoTaskViewModel task) : IUiMessage
{
    public TodoTaskViewModel Task { get; } = task;
}