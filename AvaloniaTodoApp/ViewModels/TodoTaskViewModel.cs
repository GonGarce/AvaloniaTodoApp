using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Input;
using AvaloniaTodoAPp.Memento;
using AvaloniaTodoAPp.Messages;
using AvaloniaTodoAPp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Meziantou.Framework;

namespace AvaloniaTodoAPp.ViewModels;

public partial class TodoTaskViewModel : ViewModelBase
{
    public TodoTaskViewModel(TodoTask todo)
    {
        Description = todo.Description;
        Completed = todo.Done;
        CreationDate = todo.CreationDate;
    }

    public string Description { get; }

    [ObservableProperty]
    private bool _completed;
    
    [ObservableProperty]
    private bool _important;

    [ObservableProperty]
    private DateTime? _creationDate;
    
    private string? _created;

    public string Created
    {
        get
        {
            DateTime date = CreationDate ?? DateTime.Now;
            return CreationDate != null ? RelativeDate.Get(date).ToString(format: null, new CultureInfo("es-ES")) : "";
        }
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