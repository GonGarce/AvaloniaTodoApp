using AvaloniaTodoApp.ViewModels.Collections;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoApp.Messages;

public class HideCollection(CollectionItemViewModel collection) : ValueChangedMessage<CollectionItemViewModel>(collection);