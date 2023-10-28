using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace wiseoldmanV2
{
    public class AdminMessageManager
    {
        private readonly DiscordClient _client;
        private readonly ulong _channelId = 686840457338421251; // Replace with your channel ID
        private readonly ulong _guildId = 686840457334227129; // Replace with your guild ID

        public AdminMessageManager(DiscordClient client)
        {
            _client = client;
        }

        public async Task SendAdminMessage(string messageType, string messageText)
        {
            var guild = await _client.GetGuildAsync(_guildId);

            if (guild != null)
            {
                var channel = guild.GetChannel(_channelId);

                if (channel != null)
                {
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = GetTitle(messageType),
                        Description = messageText, // No need to sanitize the message text
                        Color = GetEmbedColor(messageType),
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            Text = $"Sent by AdminPanel at {DateTime.Now:dd-MM-yyyy HH:mm:ss}",
                        },
                    };

                    await channel.SendMessageAsync(embed: embed);
                }
            }
        }



        private string GetTitle(string messageType)
        {
            switch (messageType.ToLower())
            {
                case "announcement":
                    return "📢 Announcement";

                case "test":
                    return "🧪 Test Message";

                case "update":
                    return "🛠️ Update";

                default:
                    return "Message";
            }
        }

        private DiscordColor GetEmbedColor(string messageType)
        {
            switch (messageType.ToLower())
            {
                case "announcement":
                    return new DiscordColor(0, 255, 0); // Green color

                case "test":
                    return new DiscordColor(255, 255, 0); // Yellow color

                case "update":
                    return new DiscordColor(255, 0, 0); // Red color

                default:
                    return new DiscordColor(0, 0, 255); // Blue color
            }
        }
    }
}
