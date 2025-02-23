using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaTodoApp.App;
using AvaloniaTodoAPp.Models;
using AvaloniaTodoAPp.ViewModels;
using AvaloniaTodoApp.Views;
using AvaloniaTodoAPp.Views;
using Supabase.Gotrue;

namespace AvaloniaTodoAPp.Client;

public class LoginPage
{
    public static async void LoginOrLoad()
    {
        AppState.Instance.Supabase.Auth.LoadSession(); //Load session
        var session = await AppState.Instance.Supabase.Auth.RetrieveSessionAsync();
        if (session == null)
        {
            //await Shell.Current.GoToAsync(nameof(MyPageWelcome)); //Page if session is null or has expired.
            WindowManager.ChangeWindow(() => new LoginWindow { DataContext = new LoginWindowViewModel() });
        }
        else
        {
            //await Shell.Current.GoToAsync(nameof(MyMainPage)); //Everything Ok
            var profile  = await AppState.Instance.Supabase.From<SProfile>()
                .Where(profile => profile.Id == AppState.Instance.UserId)
                .Single();
            AppState.Instance.Profile = profile!;
            WindowManager.ShowMainWindow();
        }
    }

    public static async Task Register(string name, string password, string username)
    {
        try
        {
            await AppState.Instance.Supabase.Auth.SignIn(name, password);
            WindowManager.ShowMainWindow();
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        var session = await AppState.Instance.Supabase.Auth.SignUp(name, password, new SignUpOptions()
        {
            Data = new Dictionary<string, object>
            {
                { "username", username }
            }
        });
        //var session1 = AppState.Instance.Supabase.Auth.CurrentSession;
        if (session != null)
        {
            //var newPersistence = new SessionHandler();
            //newPersistence.SaveSession(session);
            //AppState.Instance.Supabase.Auth.SetPersistence(newPersistence);
            //await Shell.Current.GoToAsync(nameof(MyPageAfterRegister));
            WindowManager.ShowMainWindow();
        }
        else
        {
            //Msg error
            WindowManager.ChangeWindow(() => new SplashWindow());
        }
    }
}