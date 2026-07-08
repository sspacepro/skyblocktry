
using System.IO;
using System.Text.Json;

namespace SkyBlockTray;
//dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
public class Settings
{
    public string ApiKey { get; set; } = "";
    public static int CheckIntervalSeconds = 60;


    public static Settings Load()
    {
        string file = "settings.json";

        if (!File.Exists(file))
        {
            File.WriteAllText(
                file,
                """
                {
                  "ApiKey": "PUT_YOUR_API_KEY_HERE"
                }
                """
            );
        }


        string json = File.ReadAllText(file);

        return JsonSerializer.Deserialize<Settings>(json)
               ?? new Settings();
    }
}