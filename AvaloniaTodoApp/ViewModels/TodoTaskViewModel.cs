using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Input;
using AvaloniaTodoApp.Memento;
using AvaloniaTodoApp.Messages;
using AvaloniaTodoApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Meziantou.Framework;

namespace AvaloniaTodoApp.ViewModels;

public partial class TodoTaskViewModel : ViewModelBase
{
    public TodoTaskViewModel(STask todo)
    {
        Id = todo.Id;
        Description = todo.Description;
        CompletedAt = todo.CompletedAt;
        Completed = todo.IsCompleted();
        CreationDate = todo.CreatedAt;
        Important = todo.Critical;
        CollectionId = todo.CollectionId;
    }

    public TodoTaskViewModel(string description, int collectionId)
    {
        Id = Guid.NewGuid();
        Description = description;
        CreationDate = DateTime.Now;
        IsLoading = true;
        CollectionId = collectionId;
    }

    public string Description { get; }

    public Guid Id { get; set; }
    
    public DateTime? CompletedAt;
    
    public int CollectionId;

    [ObservableProperty]
    private bool _completed;

    [ObservableProperty]
    private bool _important;

    [ObservableProperty]
    private DateTime _creationDate;
    
    [ObservableProperty]
    private bool _isLoading;

    private string? _created;

    public string Created
    {
        get => RelativeDate.Get(CreationDate.ToUniversalTime()).ToString();
        set => SetProperty(ref _created, value);
    }

    [RelayCommand]
    private void Delete()
    {
        WeakReferenceMessenger.Default.Send(new RemoveTodoTaskMessage(this));
    }

    [RelayCommand]
    public void ToggleCompleted()
    {
        WeakReferenceMessenger.Default.Send(new ChangedTodoTaskMessage(new CommandToggleDone(this)));
    }

    [RelayCommand]
    private void ToggleImportant()
    {
        WeakReferenceMessenger.Default.Send(new ChangedTodoTaskMessage(new CommandToggleImportant(this)));
    }
}