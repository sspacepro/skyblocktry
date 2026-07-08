using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SkyBlockTray;

public static class SkyBlockStatusService
{
    private static readonly HttpClient client = new();

    private const string ApiKey = "9f72bcb4-0b7f-4615-9d7c-2b2e7d100a4f";


    public static async Task<int> GetSkyBlockPlayerCount()
    {
        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.hypixel.net/v2/counts"
            );

            request.Headers.Add("API-Key", ApiKey);


            using var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return -1;


            string json = await response.Content.ReadAsStringAsync();


            using JsonDocument doc = JsonDocument.Parse(json);


            var skyblock = doc.RootElement
                .GetProperty("games")
                .GetProperty("SKYBLOCK");


            return skyblock
                .GetProperty("players")
                .GetInt32();
        }
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    return -1;
}
    }
}