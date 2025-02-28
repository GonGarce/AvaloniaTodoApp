using AvaloniaTodoApp.ViewModels.Collections;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AvaloniaTodoAPp.Messages;

public class ChangeCurrentCollection(CollectionItemViewModel collection) : ValueChangedMessage<CollectionItemViewModel>(collection);