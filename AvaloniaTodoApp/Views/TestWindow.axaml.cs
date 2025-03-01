using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AvaloniaTodoApp.Views.Dialogs;

namespace AvaloniaTodoApp.Views;

public partial class TestWindow : Window
{
    public TestWindow()
    {
        InitializeComponent();
    }
    
    private async Task InteractionHandler(/*MusicStoreViewModel vm*/)
    {
        // Get a reference to our TopLevel (in our case the parent Window)
        //var topLevel = TopLevel.GetTopLevel(this);

        var dialog = new InviteWindow();
        //dialog.DataContext = vm;

        var result = await dialog.ShowDialog<object?>(this);
        
        //return result;
    }

    private async void TapInvite(object? sender, TappedEventArgs e)
    {
        await InteractionHandler();
    }
}