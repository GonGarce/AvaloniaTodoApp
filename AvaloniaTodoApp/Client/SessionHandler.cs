using Supabase.Gotrue.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Gotrue;
using Newtonsoft.Json;


namespace AvaloniaTodoAPp.Client;

public class SessionHandler : IGotrueSessionPersistence<Session>
{
    private static readonly string AppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string CacheDir => $"{AppDataDir}/GonGarce/TodoApp/cache";

    private string GetCacheDir()
    {
        if (!Directory.Exists(CacheDir))
        {
            Directory.CreateDirectory(CacheDir);
        }

        return CacheDir;
    }

    public void SaveSession(Session session)
    {
        var cacheFileName = ".gotrue.cache";

        try
        {
            var cacheDir = GetCacheDir(); //FileSystem.CacheDirectory;
            var path = Path.Join(cacheDir, cacheFileName);
            var str = JsonConvert.SerializeObject(session);

            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write(str);
                //file.Dispose();
            }

            ;
            Console.WriteLine("!--------------SAVED SESSION--------------!");
        }
        catch (Exception err)
        {
            Console.WriteLine("Unable to write cache file." + err);
        }
    }

    public void DestroySession()
    {
        // Destroy Session on Filesystem or in browser storage
        var cacheFileName = ".gotrue.cache";
        var cacheDir = GetCacheDir(); //FileSystem.CacheDirectory;
        var path = Path.Join(cacheDir, cacheFileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        //Other logic Delete cache

        Console.WriteLine("!--------------DESTROY SESSION--------------!");
    }

    public Session? LoadSession()
    {
        try
        {
            var cacheFileName = ".gotrue.cache";
            var cacheDir = GetCacheDir(); //FileSystem.CacheDirectory;
            var path = Path.Join(cacheDir, cacheFileName);
            using StreamReader r = new(path);
            string json = r.ReadToEnd();
            // Retrieve Session from Filesystem or from browser storage
            Console.WriteLine("!--------------LOAD SESSION--------------!");
            return JsonConvert.DeserializeObject<Session>(json);
        }
        catch (Exception err)
        {
            Console.WriteLine("Unable to write cache file." + err);
            return null;
        }
    }
}