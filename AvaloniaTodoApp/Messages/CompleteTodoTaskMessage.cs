using AvaloniaTodoAPp.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoAPp.Messages;

public class CompleteTodoTaskMessage(TodoTaskViewModel value) : ValueChangedMessage<TodoTaskViewModel>(value);