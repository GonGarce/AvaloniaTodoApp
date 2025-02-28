using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using AvaloniaTodoApp;
using AvaloniaTodoApp.Global;
using AvaloniaTodoApp.Messages;
using AvaloniaTodoApp.Models;
using AvaloniaTodoApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Supabase.Realtime;
using Supabase.Realtime.PostgresChanges;

namespace AvaloniaTodoApp.ViewModels.Collections;

public partial class CollectionsTabViewModel : ViewModelBase
{
    public static readonly IEnumerable<CollectionItemViewModel> AppCollections =
    [
        new(-1, "To-Do", 0, DateTime.MinValue, false, true),
        new(-2, "Critical", 0, DateTime.MinValue, false, true)
    ];

    private RealtimeChannel? _channel; // TODO: Unsubscribe

    public CollectionsTabViewModel()
    {
        if (Design.IsDesignMode) return;

        SubscribeChannel();

        AppState.Instance.Supabase.From<SCollection>()
            .Order(col => col.CreatedAt, Supabase.Postgrest.Constants.Ordering.Ascending)
            .Get()
            .ContinueWith(task =>
            {
                var tabs = task.Result.Models
                    .Select(Mapper.ToViewModel);
                Dispatcher.UIThread.Post(() => SetTabs(tabs));
            });
        WeakReferenceMessenger.Default.Register<HideCollection>(this,
            (_, message) => Dispatcher.UIThread.Post(() => Tabs.Remove(message.Value)));
    }

    [ObservableProperty]
    private ObservableCollection<CollectionItemViewModel> _tabs = [];

    [ObservableProperty]
    private CollectionItemViewModel _selectedTab = AppCollections.First(); // TODO: Remember

    [ObservableProperty]
    private string _textNewList = string.Empty;

    [ObservableProperty]
    private bool _isAdding;

    partial void OnSelectedTabChanged(CollectionItemViewModel value)
    {
        MainWindowState.Instance().ChangeCollection(value);
    }

    [RelayCommand]
    private void SetNewListStatus(bool show)
    {
        IsAdding = show;
        TextNewList = "";
    }


    [RelayCommand]
    private void AddNewList()
    {
        IsAdding = false;
        int order = Tabs.Any() ? Tabs.Max(col => col.Order) + 1 : 1;
        AppState.Instance.Supabase.From<SCollection>()
            .Insert(new SCollection()
            {
                Name = TextNewList,
                Order = order,
                CreatedAt = DateTime.Now,
                OwnerId = AppState.Instance.UserId
            });
    }

    [RelayCommand]
    private void ShowAll()
    {
        for (int i = 0; i < AppCollections.Count(); i++)
        {
            // Reinsert default collections
            // The order should be the same so, as you are modifying Tabs list, you can just check index by index
            var defCol = AppCollections.ElementAt(i);
            if (Tabs[i] != defCol) Tabs.Insert(i, defCol);
        }
    }

    [RelayCommand]
    private void SwitchTheme()
    {
        ThemeManager.ToggleTheme();
    }

    private void SetTabs(IEnumerable<CollectionItemViewModel> tabs)
    {
        Tabs = new ObservableCollection<CollectionItemViewModel>(AppCollections.Concat(tabs));
    }

    private async void SubscribeChannel()
    {
        _channel = await AppState.Instance.Supabase.From<SCollection>().On(PostgresChangesOptions.ListenType.All,
            (_, change) =>
            {
                if (change.Payload?.Data?.Type is Constants.EventType.Insert)
                {
                    var col = change.Model<SCollection>()!;
                    var item = Mapper.ToViewModel(col);
                    Dispatcher.UIThread.Post(() => { Tabs.Add(item); });
                }

                if (change.Payload?.Data?.Type is Constants.EventType.Update)
                {
                    var col = change.Model<SCollection>()!;
                    Dispatcher.UIThread.Post(() =>
                    {
                        var collection = Tabs.First(c => c.Id == col.Id);
                        collection.Order = col.Order;
                        collection.Title = col.Name;
                    });
                }

                if (change.Payload?.Data?.Type == Constants.EventType.Delete)
                {
                    var col = change.OldModel<SCollection>()!;
                    var item = Tabs.First(c => c.Id == col.Id);
                    Dispatcher.UIThread.Post(() => { Tabs.Remove(item); });
                }
            });
    }
}