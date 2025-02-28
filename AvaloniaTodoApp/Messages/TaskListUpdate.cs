using System.Collections.Generic;
using AvaloniaTodoApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoApp.Messages;

public class TaskListUpdate(List<TodoTaskViewModel> list) : ValueChangedMessage<List<TodoTaskViewModel>>(list);