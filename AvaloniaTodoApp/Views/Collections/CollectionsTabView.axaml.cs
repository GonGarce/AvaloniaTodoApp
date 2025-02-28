using Avalonia.Controls;

namespace AvaloniaTodoApp.Views.Collections;

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