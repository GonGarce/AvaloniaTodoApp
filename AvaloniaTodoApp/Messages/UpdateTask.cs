using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Messages;

public class UpdateTask(TodoTaskViewModel task) : IUiMessage
{
    public TodoTaskViewModel Task { get; } = task;
}