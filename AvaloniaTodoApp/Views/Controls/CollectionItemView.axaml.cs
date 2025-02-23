using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaTodoAPp.Dialogs;
using DialogHostAvalonia;

namespace AvaloniaTodoApp.Views.Controls;

public partial class CollectionItemView : UserControl
{
    public CollectionItemView()
    {
        InitializeComponent();

        TextEdit.PropertyChanged += (s, e) =>
        {
            if (e.Property.Name == "IsVisible" && (bool)(e.NewValue ?? false))
            {
                TextEdit.Focus();
                TextEdit.SelectAll();
            }
        };
    }

    public void OnDoubleTapItem(object? sender, TappedEventArgs tappedEventArgs)
    {
        StartEdit.Command?.Execute(true);
    }

    public void OnTextEditLostFocus(object? sender, RoutedEventArgs routedEventArgs)
    {
        FinishEdit.Command?.Execute(false);
    }

    public void OpenDeleteDialog(object? sender, TappedEventArgs tappedEventArgs)
    {
        //await DialogHost.Show(new DeleteDialogModel(1, "Regalos para Sabela"), "CollectionsDialogHost");
    }
}