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
        private readonly ulong _reportChannelId = 715198677173534743; 
        private ulong _reportguildID = 715197288984870932; 

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

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Reading Message content from user ID {e.Message.Author.Id} - {e.Message.Author.Username}");
            Console.WriteLine("Checking permissions...");

          
            Console.WriteLine($"User is part of {sharedGuilds.Count()} shared guild(s):");

            foreach (var sharedGuild in sharedGuilds)
            {
                Console.WriteLine($"- Guild Name: {sharedGuild.Value.Name}, Guild ID: {sharedGuild.Key}");
            }

            if (!sharedGuilds.Any())
            {
                Console.WriteLine("User does not have permission to use the W00ters Reporting function.");
                Console.ResetColor();

                // Return a DM message to the user
                var noPermissionEmbed = new DiscordEmbedBuilder
                {
                    Title = "Permission Denied",
                    Description = "You do not have permission to use the W00ters Reporting function.",
                    Color = new DiscordColor(255, 0, 0) 
                };

                await e.Message.RespondAsync(embed: noPermissionEmbed);
                return;
            }

            if (messageContent.StartsWith("!report"))
            {
                var reportMessage = messageContent.Substring("!report".Length).Trim();
                var guild = await _client.GetGuildAsync(_reportguildID);

                if (guild != null)
                {
                    var reportChannel = guild.GetChannel(_reportChannelId);

                    if (reportChannel != null)
                    {
                        var userAvatarUrl = e.Message.Author.AvatarUrl;
                        var userAvatar = userAvatarUrl != null ? new DiscordEmbedBuilder.EmbedThumbnail { Url = userAvatarUrl } : null;

                        var embed = new DiscordEmbedBuilder
                        {
                            Title = "🚨 Incoming W00ters Report! 🚨",
                            Description = reportMessage,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = $"Reported by {e.Message.Author.Username} at {DateTime.Now:dd-MM-yyyy HH:mm:ss}"
                            },
                            Color = new DiscordColor(255, 0, 0)
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
                    Color = new DiscordColor(0, 255, 0)
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
                    Color = new DiscordColor(0, 255, 0)
                };

                await e.Message.RespondAsync(embed: pingResponseEmbed);
            }
            else if (messageContent.StartsWith("!version"))
            {
                // You'll need to use an HTTP client to fetch the version from GitHub.
                // I'll provide an example using HttpClient.

                using (var httpClient = new HttpClient())
                {
                    // GitHub raw URL to the version file.
                    string githubVersionUrl = "https://raw.githubusercontent.com/Nathant995/WiseOldManV2/main/wiseoldmanV2/loader.cs";

                    try
                    {
                        // Send a GET request to fetch the content of the version file.
                        var response = await httpClient.GetAsync(githubVersionUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            // Read the content of the file.
                            string versionFileContent = await response.Content.ReadAsStringAsync();

                            // Search for the version line and extract the version number.
                            string versionLine = versionFileContent.Split('\n').FirstOrDefault(line => line.Contains("string version"));
                            string version = versionLine?.Split('"')[1]; // Extract the version number between quotes.

                            if (version != null)
                            {
                                // Respond with the bot's version.
                                var versionResponseEmbed = new DiscordEmbedBuilder
                                {
                                    Title = "Wise Old Man Build",
                                    Description = $"I am currently operating on build: **{version}**.",
                                    Color = new DiscordColor(108, 202, 255)
                                };

                                await e.Message.RespondAsync(embed: versionResponseEmbed);
                            }
                            else
                            {
                                // If the version line was not found in the file, respond with an error.
                                await e.Message.RespondAsync("Failed to retrieve the bot's version.");
                            }
                        }
                        else
                        {
                            // If the request to GitHub fails, respond with an error.
                            await e.Message.RespondAsync("Failed to retrieve the bot's version.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that may occur during the request.
                        await e.Message.RespondAsync($"An error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Non-command DM Received from; {e.Message.Author.Id} - {e.Message.Author.Username}");
                Console.WriteLine("Attempting to handle Direct Message");
                Console.ForegroundColor = ConsoleColor.White;
                // If the message doesn't match any recognized command, respond with an embedded message and a GIF
                var gifUrl = "https://media3.giphy.com/media/3o6MbbwX2g2GA4MUus/giphy.gif?cid=ecf05e47zwv1rsrkmdxhb2ohcutyla5f7bn8ezynnc8jqpdh&ep=v1_gifs_search&rid=giphy.gif&ct=g";
                var embed = new DiscordEmbedBuilder
                {
                    Title = "I don't understand how to process that message...",
                    Description = "Try using a ! prefix to your message or ask Nath for support.",
                    ImageUrl = gifUrl,
                    Color = new DiscordColor(255, 0, 0)
                };

                await e.Message.RespondAsync(embed: embed);
            }
        }
    }
}
        
