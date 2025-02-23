using System.Collections.Generic;
using AvaloniaTodoAPp.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoAPp.Messages;

public class TaskListUpdate(List<TodoTaskViewModel> list) : ValueChangedMessage<List<TodoTaskViewModel>>(list);