using AvaloniaTodoApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoApp.Messages;

public class RemoveTodoTaskMessage(TodoTaskViewModel value) : ValueChangedMessage<TodoTaskViewModel>(value);