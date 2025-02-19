using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaTodoAPp.ViewModels;

namespace AvaloniaTodoAPp.Views;

public partial class TodoTaskView : UserControl
{
    public TodoTaskView()
    {
        InitializeComponent();
    }
    
    private TodoTaskViewModel? _vm;
    
    private void ToggleCompleted(object sender, TappedEventArgs args)
    {
        if (_vm != null)
        {
            _vm.ToggleCompleted();
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        Debug.Print("MusicStoreView:OnDataContextChanged");
        //_vm?.Dispose();

        if (DataContext is TodoTaskViewModel vm)
        {
            _vm = vm;
        }

        base.OnDataContextChanged(e);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        //_vm?.Dispose();
        base.OnUnloaded(e);
    }
}