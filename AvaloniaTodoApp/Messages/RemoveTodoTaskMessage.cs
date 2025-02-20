using AvaloniaTodoAPp.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoAPp.Messages;

public class RemoveTodoTaskMessage(TodoTaskViewModel value) : ValueChangedMessage<TodoTaskViewModel>(value);