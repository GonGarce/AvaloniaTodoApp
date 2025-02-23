using System.Threading.Tasks;
using AvaloniaTodoAPp.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaTodoAPp.ViewModels;

public partial class LoginWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _email = "";
    
    [ObservableProperty]
    private string _username = "";

    [ObservableProperty]
    private string _password = "";

    [RelayCommand]
    public async Task Login()
    {
        await LoginPage.Register(Email, Password, Username);
    }
}