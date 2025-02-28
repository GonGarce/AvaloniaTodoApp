using System.Threading.Tasks;
using AvaloniaTodoApp.Global;
using AvaloniaTodoApp.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaTodoApp.ViewModels;

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
        var credentials = ConfigManager.GetUserCredentials();
        Email = credentials.Url;
        Password = credentials.ApiKey;
        await LoginPage.Register(Email, Password, Username);
    }
}