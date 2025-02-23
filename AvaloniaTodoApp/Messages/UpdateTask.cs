using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Messages;

public class UpdateTask(TodoTaskViewModel task) : IUiMessage
{
    public TodoTaskViewModel Task { get; } = task;
}