using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Avalonia.Controls.Notifications;
using AvaloniaTodoApp.App;
using Microsoft.Extensions.Configuration;
using Supabase.Gotrue;

namespace AvaloniaTodoAPp.Client;

using System.Threading.Tasks;
using Client = Supabase.Client;

public static class SupabaseService
{
    private static void DebugHandler(string category, string message, Exception? exception)
    {
        Console.WriteLine($"[{category}]: {message}");
        Debugger.Log(1, category, exception != null ? exception.Message : message);
    }

    public static async Task<Client> InitService()
    {
        var credentials = ConfigManager.GetServerCredentials();
        string settingsUrl = $"{credentials.Url}/auth/v1/settings?apikey={credentials.ApiKey}";

        var sessionHandler = new SessionHandler();
        var supabase = new Client(credentials.Url, credentials.ApiKey, new Supabase.SupabaseOptions
        {
            // Set the options to auto-refresh the JWT token with a background thread.
            AutoRefreshToken = true,
            AutoConnectRealtime = true,
            SessionHandler = sessionHandler, //You can use another Implementation, only change the logic in the file.
        });

        // This adds a listener for debug information, especially useful for dealing
        // with errors from the auto-refresh background thread.
        supabase.Auth.AddDebugListener((message, exception) =>
            DebugHandler("Supabase AUTH", $"{message}", exception));
        supabase.Postgrest.AddDebugHandler((sender, message, exception) =>
            DebugHandler("Supabase Postgrest", $"{message}", exception));
        //supabase.Realtime.AddDebugHandler((sender, message, exception) => DebugHandler("Supabase Realtime", $"{message}", exception));

        // We create an object to monitor the network status.
        NetworkStatus networkStatus = new();

        // Here we are getting the auth service and passing it to the network status
        // object. The network status object will tell the auth service when the
        // network is up or down.
        networkStatus.Client = (Supabase.Gotrue.Client)supabase.Auth;

        // In this case, we are setting up the auth service to allow unconfirmed user sessions.
        // Depending on your use case, you may want to set this to false and require the user
        // to validate their email address before they can log in.
        supabase.Auth.Options.AllowUnconfirmedUserSessions = true;

        // Enough with SessionHandler on creation options?
        //supabase.Auth.SetPersistence(sessionHandler);

        // TODO: Use State change listener
        // Here we are setting up the listener for the auth state. This listener will
        // be called in response to the auth state changing. This is where you would
        // update your UI to reflect the current auth state.
        supabase.Auth.AddStateChangedListener((sender, changed) => { Debugger.Log(1, "DEBUG", changed.ToString()); });

        try
        {
            // We start the network status object. This will attempt to connect to the
            // well-known URL and determine if the network is up or down.
            // Start monitoring and get initial state
            supabase.Auth.Online = await networkStatus.StartAsync(settingsUrl);
        }
        catch (NotSupportedException)
        {
            // On certain platforms, the NetworkStatus object may not be able to determine
            // the network status. In this case, we just assume the network is up.
            supabase.Auth.Online = true;
        }
        catch (Exception e)
        {
            Debugger.Log(1, "DEBUG", e.Message);
            // If there are other kinds of error, we assume the network is down,
            // and in this case we send the error to a UI element to display to the user.
            // This PostMessage method is specific to this application - you will
            // need to adapt this to your own application.
            //PostMessage(NotificationType.Debug, $"Network Error {e.GetType()}", e);
            // TODO: Notify error. Maybe https://github.com/AvaloniaCommunity/Notification.Avalonia?tab=readme-ov-file
            // Debugger.Log(1, "DEBUG", exception != null ? exception.Message : "AddDebugListener");
            supabase.Auth.Online = false;
        }

        await supabase.InitializeAsync();

        return supabase;
    }
}