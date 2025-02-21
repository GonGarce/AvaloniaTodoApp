using Avalonia.Controls;

namespace AvaloniaTodoAPp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NewTaskInput.AttachedToVisualTree += (s, e) => NewTaskInput.Focus();
        SearchButton.Command = new CommunityToolkit.Mvvm.Input.RelayCommand(FocusTextBox);;
    }

    private void FocusTextBox()
    {
        NewTaskInput.Focus();
    }
}