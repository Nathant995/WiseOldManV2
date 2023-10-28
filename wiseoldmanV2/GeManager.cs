using DSharpPlus.Entities;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using System.Windows.Forms;

public class GeManager
{
    private const string BaseUrl = "https://services.runescape.com/m=itemdb_rs/api";

    public async Task<DiscordEmbedBuilder> GetItemInfoByIdAsync(int itemId)
    {
        // Fetch item information by item ID
        string itemInfoUrl = $"{BaseUrl}/catalogue/detail.json?item={itemId}";
        string itemInfoJson = await FetchDataAsync(itemInfoUrl);

        // Parse item information from JSON
        var itemInfo = JObject.Parse(itemInfoJson)["item"];

        string name = itemInfo["name"].ToString();
        string description = itemInfo["description"].ToString();
        string type = itemInfo["type"].ToString();
        string iconUrl = itemInfo["icon_large"].ToString();
        bool members = bool.Parse(itemInfo["members"].ToString());

        // Create an embed with the item information
        var embedBuilder = new DiscordEmbedBuilder()
            .WithTitle(name)
            .WithDescription(description)
            .AddField("Type", type, true)
            .AddField("Members Only", members.ToString(), true)
            .WithImageUrl(iconUrl)
            .WithColor(DiscordColor.Green);

        return embedBuilder;
    }

    public async Task<string> GetItemPriceGraphUrlAsync(int itemId)
    {
        // Fetch price graph information by item ID
        string priceGraphUrl = $"{BaseUrl}/graph/{itemId}.json";
        return priceGraphUrl;
    }

    private async Task<string> FetchDataAsync(string url)
    {
        using (var httpClient = new HttpClient())
        {
            return await httpClient.GetStringAsync(url);
        }
    }
}