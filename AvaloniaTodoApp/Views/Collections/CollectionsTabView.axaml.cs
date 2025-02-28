using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaTodoApp.Views.Controls;

public partial class CollectionsTabView : UserControl
{
    public CollectionsTabView()
    {
        InitializeComponent();

        TextNewList.PropertyChanged += (s, e) =>
        {
            if (e.Property.Name == "IsVisible" && (bool)(e.NewValue ?? false))
            {
                TextNewList.Focus();
            }
        };
    }
}