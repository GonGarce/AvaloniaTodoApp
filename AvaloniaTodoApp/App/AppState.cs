using System.Globalization;
using System.Threading.Tasks;
using AvaloniaTodoAPp.Client;
using AvaloniaTodoAPp.Models;

namespace AvaloniaTodoApp.App;

public class AppState
{
    public const bool Test = false;

    private AppState()
    {
    }

    public static AppState Instance { get; private set; } = new();

    public Supabase.Client Supabase { get; set; }

    public string UserId => Supabase.Auth.CurrentUser!.Id!;

    public SProfile Profile { get; set; }

    public async Task Initialize()
    {
        Supabase = await SupabaseService.InitService();
    }
}