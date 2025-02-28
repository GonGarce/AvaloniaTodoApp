using AvaloniaTodoApp.Memento;
using AvaloniaTodoApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoApp.Messages;

public class ChangedTodoTaskMessage(IMCommand command) : ValueChangedMessage<IMCommand>(command);