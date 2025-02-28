using System;
using Microsoft.Extensions.Configuration;

namespace AvaloniaTodoApp.App;

public static class ConfigManager
{
    public static ServerCredentials GetServerCredentials()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile(".env/settings.json")
            .Build();
        IConfigurationSection section = config.GetSection("Supabase");
        string supabaseUrl = section["url"] ?? throw new InvalidOperationException();
        string apiKey = section["key"] ?? throw new InvalidOperationException();
        return new ServerCredentials(supabaseUrl, apiKey);
    }

    // TODO: Remove testing purpouses
    public static ServerCredentials GetUserCredentials()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile(".env/settings.json")
            .Build();
        IConfigurationSection section = config.GetSection("Credentials");
        string supabaseUrl = section["email"] ?? throw new InvalidOperationException();
        string apiKey = section["password"] ?? throw new InvalidOperationException();
        return new ServerCredentials(supabaseUrl, apiKey);
    }
}