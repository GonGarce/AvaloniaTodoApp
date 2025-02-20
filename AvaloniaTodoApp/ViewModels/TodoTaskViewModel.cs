using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Input;
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
    private DateTime? _creationDate;
    
    private string _created;

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
    public void ToggleCompleted()
    {
        Completed = !Completed;
    }
}