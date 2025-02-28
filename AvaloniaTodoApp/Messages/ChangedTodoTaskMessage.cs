﻿using AvaloniaTodoAPp.Memento;
using AvaloniaTodoAPp.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoAPp.Messages;

public class ChangedTodoTaskMessage(IMCommand command) : ValueChangedMessage<IMCommand>(command);