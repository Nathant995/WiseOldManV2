using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace wiseoldmanV2
{
    public class DMManager
    {
        private readonly DiscordClient _client;
        private readonly ulong _reportChannelId = 715198677173534743; // Replace with the channel ID that reports need to be sent to (for now it's in the testing channel in the test discord)
        private ulong _reportguildID = 715197288984870932; /// Replace with the guild ID that reports need to be sent to (for now it's in the testing discord)

        public DMManager(DiscordClient client)
        {
            _client = client;

            _client.MessageCreated += HandleDmAsync;
        }

        private async Task HandleDmAsync(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Message.Author.IsBot || e.Message.Channel.IsPrivate == false) return; // Ignore messages from bots and non-DM channels

            var messageContent = e.Message.Content.ToLower();

            // Check if the reporting user is part of an allowed guild
            var sharedGuilds = _client.Guilds.Where(g => g.Value.Members.ContainsKey(e.Message.Author.Id) && g.Value.Members.ContainsKey(_client.CurrentUser.Id));

            Console.ForegroundColor = ConsoleColor.Blue; // Set the console text color to blue
            Console.WriteLine($"Reading Message content from user ID {e.Message.Author.Id} - {e.Message.Author.Username}");
            Console.WriteLine("Checking permissions...");

            // Display information about shared guilds
            Console.WriteLine($"User is part of {sharedGuilds.Count()} shared guild(s):");

            foreach (var sharedGuild in sharedGuilds)
            {
                Console.WriteLine($"- Guild Name: {sharedGuild.Value.Name}, Guild ID: {sharedGuild.Key}");
            }

            if (!sharedGuilds.Any())
            {
                Console.WriteLine("User does not have permission to use the W00ters Reporting function.");
                Console.ResetColor(); // Reset the console text color

                // Return a DM message to the user
                var noPermissionEmbed = new DiscordEmbedBuilder
                {
                    Title = "Permission Denied",
                    Description = "You do not have permission to use the W00ters Reporting function.",
                    Color = new DiscordColor(255, 0, 0) // Red color
                };

                await e.Message.RespondAsync(embed: noPermissionEmbed);
                return;
            }

            if (messageContent.StartsWith("!report"))
            {
                var reportMessage = messageContent.Substring("!report".Length).Trim();
                var guild = await _client.GetGuildAsync(_reportguildID); // Replace with your guild ID

                if (guild != null)
                {
                    var reportChannel = guild.GetChannel(_reportChannelId);

                    if (reportChannel != null)
                    {
                        var userAvatarUrl = e.Message.Author.AvatarUrl; // Get the reporting user's avatar URL
                        var userAvatar = userAvatarUrl != null ? new DiscordEmbedBuilder.EmbedThumbnail { Url = userAvatarUrl } : null;

                        var embed = new DiscordEmbedBuilder
                        {
                            Title = "🚨 Incoming W00ters Report! 🚨", // Use emojis to make it stand out
                            Description = reportMessage,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = $"Reported by {e.Message.Author.Username} at {DateTime.Now:dd-MM-yyyy HH:mm:ss}"
                            },
                            Color = new DiscordColor(255, 0, 0) // Red color
                        };

                        // Get the guilds that the user and bot share

                        var sharedGuildsList = string.Join("\n", sharedGuilds.Select(g => g.Value.Name));

                        embed.AddField("Related Guilds", sharedGuildsList, false)
                        .WithAuthor(e.Message.Author.Username, iconUrl: e.Message.Author.AvatarUrl);

                        await reportChannel.SendMessageAsync(embed: embed);
                    }
                }
                var responseEmbed = new DiscordEmbedBuilder
                {
                    Title = "Report Received",
                    Description = "Thank you for your report. The W00ters ranks have been notified.",
                    Color = new DiscordColor(0, 255, 0) // Green color
                };

                await e.Message.RespondAsync(embed: responseEmbed);
            }

            else if (messageContent.StartsWith("!ping"))
            {
                // If the message starts with "!ping", respond with "Yes, I can see you."
                var pingResponseEmbed = new DiscordEmbedBuilder
                {
                    Title = "Ping Received",
                    Description = "Yes, I can see you.",
                    Color = new DiscordColor(0, 255, 0) // Green color
                };

                await e.Message.RespondAsync(embed: pingResponseEmbed);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Non command DM Recieved from; {e.Message.Author.Id} - {e.Message.Author.Username}");
                Console.WriteLine("Attempting to handle Direct Message");
                Console.ForegroundColor = ConsoleColor.White;
                // If the message doesn't match any recognized command, respond with an embedded message and a GIF
                var gifUrl = "https://media3.giphy.com/media/3o6MbbwX2g2GA4MUus/giphy.gif?cid=ecf05e47zwv1rsrkmdxhb2ohcutyla5f7bn8ezynnc8jqpdh&ep=v1_gifs_search&rid=giphy.gif&ct=g";
                var embed = new DiscordEmbedBuilder
                {
                    Title = "I don't understand how to process that message...",
                    Description = "Try using a ! prefix to your message or ask Nath for support.",
                    ImageUrl = gifUrl,
                    Color = new DiscordColor(255, 0, 0) // Red color
                };

                await e.Message.RespondAsync(embed: embed);
            }
        }
    }
}
