namespace AvaloniaTodoApp.Global;

public class ServerCredentials(string url, string apikey)
{
    public string Url { get; } = url;
    public string ApiKey { get; } = apikey;
}