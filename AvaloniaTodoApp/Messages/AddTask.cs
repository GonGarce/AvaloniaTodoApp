using AvaloniaTodoApp.ViewModels;

namespace AvaloniaTodoApp.Messages;

public class AddTask(TodoTaskViewModel task) : IUiMessage
{
    public TodoTaskViewModel Task { get; } = task;
}