using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaTodoApp.App;
using AvaloniaTodoAPp.Dialogs;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using AvaloniaTodoAPp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;

namespace AvaloniaTodoApp.ViewModels.Collections;

public partial class CollectionItemViewModel(
    int id,
    string title,
    int order,
    DateTime createdAt,
    bool isOwner = false,
    bool isDefault = false) : ViewModelBase
{
    public CollectionItemViewModel() : this(0, "", 0, DateTime.Now, true)
    {
    }

    public bool IsTodo => IsDefault && Id == -1;
    public bool IsCritical => IsDefault && Id == -2;

    public int Id { get; } = id;

    public DateTime CreatedAt { get; } = createdAt;

    public bool IsDefault { get; } = isDefault;

    public IEnumerable<SProfile> Profiles { get; set; } = [];

    [ObservableProperty]
    private string _title = title;

    [ObservableProperty]
    private int _order = order;

    [ObservableProperty]
    private bool _hidden;

    [ObservableProperty]
    private string _textEdit = title;

    [ObservableProperty]
    private bool _canRemove = !isDefault && isOwner;

    [ObservableProperty]
    private bool _canHide = isDefault;

    [ObservableProperty]
    private bool _isEditing;

    [RelayCommand]
    private void SetEditStatus(bool value)
    {
        if (IsDefault) return;

        IsEditing = value;
        if (IsEditing)
        {
            TextEdit = Title;
        }
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(TextEdit)) return;

        IsEditing = false;
        Title = TextEdit;

        AppState.Instance.Supabase.From<SCollection>()
            .Where(col => col.Id == Id)
            .Set(x => x.Name, TextEdit)
            .Update();
    }

    [RelayCommand]
    private async Task Delete()
    {
        var result = await DialogHost.Show(new DeleteDialogModel(Title), "CollectionsDialogHost");

        if (result is not null && (bool)result)
        {
            await AppState.Instance.Supabase.From<SCollection>()
                .Where(col => col.Id == Id)
                .Delete();
        }
    }

    [RelayCommand]
    private void Hide()
    {
        Hidden = true;
        WeakReferenceMessenger.Default.Send(new HideCollection(this));
    }
}