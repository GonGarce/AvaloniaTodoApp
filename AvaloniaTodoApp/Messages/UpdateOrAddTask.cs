using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Messages;

public class UpdateOrAddTask(TodoTaskViewModel task) : IUiMessage
{
    public TodoTaskViewModel Task { get; } = task;
}