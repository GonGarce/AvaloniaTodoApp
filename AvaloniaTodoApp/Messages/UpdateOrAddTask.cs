using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Messages;

public class UpdateOrAddTask(TodoTaskViewModel task) : IUiMessage
{
    public TodoTaskViewModel Task { get; } = task;
}