using System;
using System.Threading.Tasks;
using System.Timers;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using System.Linq;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions; 
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata;
using System.Net.Http;


namespace wiseoldmanV2
{
    public class Bot
    {
        public DiscordClient _client;
        private SlashCommandsExtension _slashCommands;
        private System.Timers.Timer _healthCheckTimer;
        private HolidayEventManager _holidayEventManager;
        private ButtonManager _buttonManager;
        private System.Timers.Timer _cacheBuildingTimer;
        public RustMemeManager _memeManager;
        private readonly Dictionary<ulong, ulong> messageChannelMap = new Dictionary<ulong, ulong>();



        public static InteractivityExtension Interactivity { get; private set; }



        public DiscordClient Client
        {
            get { return _client; }
        }

        public async Task StartAsync()
        {
            try
            {
                await Task.Delay(2000);

                string token = "{TOKEN}";

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Starting bot with Token: " + token);
                _client = new DiscordClient(new DiscordConfiguration
                {
                    Token = token,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.All
                });
                _buttonManager = new ButtonManager(_client);
                _client.Ready += Client_ReadyAsync;
                _buttonManager.Initialize();



                _slashCommands = _client.UseSlashCommands();



                _slashCommands.RegisterCommands<UtilitySlashCommands>();
                _slashCommands.RegisterCommands<RuneScapeSlashCommands>();
                _slashCommands.RegisterCommands<RankInteractionCommands>();
                _slashCommands.RegisterCommands<KOSCommands>();




                // Create an instance of DMManager and pass the _client to it
                var dmManager = new DMManager(_client);
                // Create an instance of HolidayEventManager and pass the _client to it
                _holidayEventManager = new HolidayEventManager(_client);
                Interactivity = _client.UseInteractivity(new InteractivityConfiguration());
                await _client.ConnectAsync();

                // Initialize and start the health check timer
                _healthCheckTimer = new System.Timers.Timer(900000); 
                _healthCheckTimer.Elapsed += HealthCheckTimer_Elapsed;
                _healthCheckTimer.AutoReset = true;
                _healthCheckTimer.Start();

                //Rust Meme Cache Builder;

                ulong targetGuildId = 715197288984870932; 
                ulong targetChannelId = 718127567273721897; 
                string cacheFilePath = "rustmeme.cache"; 

                _memeManager = new RustMemeManager(_client, targetGuildId, targetChannelId, cacheFilePath);
                _memeManager.BuildCacheOnStartup();
                _memeManager.StartCacheBuildingTimer();

                await Task.Delay(-1);

            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during bot startup: {ex.Message}");
            }
        }







        public async Task StopAsync()
        {
            if (_client != null)
            {
                await _client.DisconnectAsync();
                _client.Dispose();
            }
            else
            {
                Console.WriteLine("Bot client is null. It may not have been initialized properly.");
            }

            Console.WriteLine("Bot has been stopped.");
        }

        // Bot Health check method
        private async void HealthCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_client == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[HealthCheck] Bot client is null. It may not have been initialized properly.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                // Check if the bot's connection is healthy
                if (_client.Ping > 600)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[HealthCheck] Reconnecting due to high ping.");
                    Console.ForegroundColor = ConsoleColor.White;

                    await _client.ReconnectAsync();
                }
                else if (_client.CurrentUser.Presence.Status != UserStatus.Online)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[HealthCheck] Reconnecting due to bot status not being online.");
                    Console.ForegroundColor = ConsoleColor.White;

                    await _client.ReconnectAsync();
                }
                else if (_client.Guilds.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[HealthCheck] Reconnecting due to bot not being in any guilds.");
                    Console.ForegroundColor = ConsoleColor.White;

                    await _client.ReconnectAsync();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[HealthCheck] Bot is healthy.");
                    Console.WriteLine("[HealthCheck] Systems are operating as expected! w00t.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[HealthCheck] Error during health check: {ex.Message}");
                Console.WriteLine("[HealthCheck] Attempting to resolve issues and reconnect...");
                Console.ForegroundColor = ConsoleColor.White;

                #region stop & start
                StopAsync();
                StartAsync();
                #endregion
            }
        }

        private async Task Client_ReadyAsync(DiscordClient sender, ReadyEventArgs e)
        {
            Console.WriteLine($"{sender.CurrentUser.Username} is connected and ready to receive traffic!");

            await sender.UpdateStatusAsync(new DiscordActivity("RuneScape"), UserStatus.Online);
        }

        public async Task SetStatusAsync(string text, ActivityType activityType)
        {
            if (_client != null)
            {
                var activity = new DiscordActivity(text, activityType);
                await _client.UpdateStatusAsync(activity, UserStatus.Online);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Bot status set to '{activityType} {text}'.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("Bot client is null. It may not have been initialized properly.");
            }

        }

        public class UtilitySlashCommands : ApplicationCommandModule
        {


            [SlashCommand("purge", "Deletes a given amount of messages in the channel.")]
            public async Task PurgeCommand(InteractionContext ctx, [Option("amount", "The number of messages to delete.")] long amount)
            {
                var member = await ctx.Guild.GetMemberAsync(ctx.User.Id);
                Console.ForegroundColor = ConsoleColor.DarkYellow;

                // Check if the user has the administrator permission
                if (!member.PermissionsIn(ctx.Channel).HasPermission(Permissions.Administrator))
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You don't have permission to use this command.").AsEphemeral(true));
                    return;
                }

                if (amount < 1 || amount > 100)
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Invalid amount. Please specify a value between 1 and 100.").AsEphemeral(true));
                    return;
                }

                var messages = await ctx.Channel.GetMessagesAsync((int)amount);

                // Check if the command is executed in a DM channel
                if (ctx.Channel is DiscordDmChannel)
                {
                    // Delete bot responses in the DM channel
                    var botMessages = messages.Where(message => message.Author.IsBot);
                    foreach (var botMessage in botMessages)
                    {
                        await ctx.Channel.DeleteMessageAsync(botMessage);
                    }

                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Deleted {botMessages.Count()} bot responses from this DM conversation :fire:"));
                    return;
                }

                // Log the number of messages queued for deletion
                Console.WriteLine($"Queued {messages.Count} messages for deletion by {ctx.Member.DisplayName} ({ctx.User.Id}):");

                foreach (var message in messages)
                {
                    // await Task.Delay(1000);
                    // Log progress and info on messages to be deleted (excluding actual message text)
                    Console.WriteLine($"Deleting message with ID {message.Id} by {message.Author.Username} at {message.CreationTimestamp}");
                }
                Console.ForegroundColor = ConsoleColor.White;
                // Delete the messages
                await Task.Delay(1000);
                await ctx.Channel.DeleteMessagesAsync(messages);
                await Task.Delay(300);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Deleted {messages.Count} messages in {ctx.Channel.Name} by {ctx.Member.DisplayName} ({ctx.User.Id}).");
                Console.ForegroundColor = ConsoleColor.White;
                // Create an embed message indicating the number of messages deleted
                var builder = new DiscordEmbedBuilder()
                    .WithAuthor(ctx.Member.DisplayName, iconUrl: ctx.Member.AvatarUrl)
                    .WithTitle("Messages Purged! :knife:")
                    .WithDescription($"{ctx.Member.Mention} has purged {messages.Count} messages from this channel :fire:")
                    .WithColor(new DiscordColor("#ff0000"));

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(builder.Build()));

            }

            [SlashCommand("memecacheinfo", "Check the meme cache")]
            public async Task CheckCacheCommand(InteractionContext ctx)
            {
                string cacheFilePath = "rustmeme.cache"; // Replace with the path to your cache file

                // Check if the command is executed in a DM channel
                //  if (ctx.Channel is DiscordDmChannel)
                //  {
                //  await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command is not available in DM channels.").AsEphemeral(true));
                //     return;
                //  }

                // Read the contents of the cache file
                string[] cacheLines = File.ReadAllLines(cacheFilePath);

                // Parse the cache lines to extract meme information
                List<Tuple<string, DateTime>> imageCache = new List<Tuple<string, DateTime>>();
                foreach (string line in cacheLines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2 && DateTime.TryParse(parts[1], out DateTime date))
                    {
                        imageCache.Add(new Tuple<string, DateTime>(parts[0], date));
                    }
                }

                if (imageCache.Count == 0)
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("The meme cache is empty or there was a problem fetching cache data."));
                    return;
                }

                // Calculate the number of available memes
                int memeCount = imageCache.Count;

                // Find the oldest and newest memes
                DateTime oldestMemeDate = imageCache.Min(item => item.Item2);
                DateTime newestMemeDate = imageCache.Max(item => item.Item2);

                // Create an embedded message with the cache information
                var embed = new DiscordEmbedBuilder()
                    .WithTitle("Meme Cache Data:")
                    .AddField("Messages Scanned to Date", memeCount.ToString(), true)
                    .AddField("Oldest Meme Date", oldestMemeDate.ToString("yyyy-MM-dd HH:mm:ss"), true)
                    .AddField("Newest Meme Date", newestMemeDate.ToString("yyyy-MM-dd HH:mm:ss"), true)
                    .WithColor(DiscordColor.White)
                    .Build();

                
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
            }



            [SlashCommand("source", "View Wise Old Man's Open Sourcecode")]
            public async Task SourceCodeCommand(InteractionContext ctx)
            {
               
                string githubRepoUrl = "https://api.github.com/repos/Nathant995/WiseOldManV2";

               
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"[GIT] Source Code Requested from {ctx.Member.Username} in guild {ctx.Guild.Name}, channel {ctx.Channel.Name} at {DateTime.Now} - Message sent successfully.");
                Console.ForegroundColor = ConsoleColor.White;

                
                string botAvatarUrl = ctx.Client.CurrentUser.AvatarUrl;

               
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "WiseOldMan");

                HttpResponseMessage response = await httpClient.GetAsync(githubRepoUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    
                    var repoInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);
                    string repoDescription = repoInfo.description;
                    int starsCount = repoInfo.stargazers_count;

                   
                    HttpResponseMessage commitResponse = await httpClient.GetAsync(githubRepoUrl + "/commits");
                    var commitInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await commitResponse.Content.ReadAsStringAsync());
                    int commitCount = commitInfo.Count;

                    
                    HttpResponseMessage releaseResponse = await httpClient.GetAsync(githubRepoUrl + "/releases");
                    var releaseInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await releaseResponse.Content.ReadAsStringAsync());
                    int releaseCount = releaseInfo.Count;

                    
                    var embedBuilder = new DiscordEmbedBuilder
                    {
                        Title = "Under The Hood",
                        Description = $"The Wise Old Man's Source Code can be found [here](https://github.com/Nathant995/WiseOldManV2).",
                        Color = new DiscordColor(0x7289DA),
                        ImageUrl = botAvatarUrl,
                        Url = githubRepoUrl,
                    };

                    
                    embedBuilder.AddField("Description", repoDescription, false);
                    embedBuilder.AddField("Stars", starsCount.ToString(), false);
                    embedBuilder.AddField("Total Commits", commitCount.ToString(), false);
                    embedBuilder.AddField("Total Releases", releaseCount.ToString(), false);

                    var ephemeralResponse = new DiscordInteractionResponseBuilder()
                        .AddEmbed(embedBuilder.Build())
                        .AsEphemeral(true);

                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, ephemeralResponse);
                }
            }





            [SlashCommand("noranks", "No ranks online reporting tool")]
            public async Task NoRanksCommand(InteractionContext ctx)
            {
                ulong targetGuildId = 715197288984870932;
                ulong targetChannelId = 715198677173534743;

                var embed = new DiscordEmbedBuilder
                {
                    Title = "Alert!",
                    Description = "There are currently no ranks online in the friends chat!",
                    Color = DiscordColor.Orange,
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        Name = ctx.Member.DisplayName,
                        IconUrl = ctx.Member.AvatarUrl
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = $"*Sent by {ctx.Member.DisplayName} from server: {ctx.Guild.Name}*"
                    },
                    ImageUrl = "https://media.giphy.com/media/1luXLMeNxsaNFMUuOe/giphy.gif"
                };

                var guild = await ctx.Client.GetGuildAsync(targetGuildId);
                if (guild is not null)
                {
                    var channel = guild.GetChannel(targetChannelId);
                    if (channel is not null && channel is DiscordChannel textChannel)
                    {
                        await textChannel.SendMessageAsync(embed: embed);
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("W00ters ranks have been notified, \n \n **You do not need to send another.** \n \n Thank you! \n \n *Wooters Ranks*"));

                        
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"[ALERT] Command executed by {ctx.Member.Username} in guild {ctx.Guild.Name}, channel {textChannel.Name} at {DateTime.Now} - Message sent successfully.");
                    }
                    else
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("Oops, I was unable to send that report! Try again later."));

                       
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"[ALERT] Command executed by {ctx.Member.Username} in guild {ctx.Guild.Name} at {DateTime.Now} - Could not find the specified channel, does the bot have permissions \n Maybe the channel does not exist?");
                    }
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Oops, I was unable to send that report! I'm having trouble connecting to the guild. Try again later."));

                    
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[ALERT] Command executed by {ctx.Member.Username} at {DateTime.Now} - Could not find the specified guild. (Check if the bot is part of W00ters Rank Hub)");
                }

               
                Console.ForegroundColor = ConsoleColor.White;
            }



            [SlashCommand("events", "Upcoming events")]
            public async Task EventsCommand(InteractionContext ctx)
            {
                var eventManager = new HolidayEventManager(ctx.Client);
                var events = eventManager.GetUpcomingEvents();
                var greenButton = new DiscordButtonComponent(ButtonStyle.Success, "addEvent", "Add Event");

                if (events.Count == 0)
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("No upcoming events found."));
                    return;
                }

                var embed = new DiscordEmbedBuilder
                {
                    Title = "Upcoming Events",
                    Color = DiscordColor.White,
                };

                foreach (var holidayEvent in events)
                {
                    string eventName = holidayEvent.Name;
                    switch (eventName)
                    {
                        case "Christmas":
                            eventName += " 🎄";
                            break;
                        case "Halloween":
                            eventName += " 🎃";
                            break;
                        case "New Year's Day":
                            eventName += " 🎆";
                            break;
                    }

                    // Convert the event date to a Unix timestamp -- Needed for discord auto countown time formatting in messages.
                    var unixTimestamp = ((DateTimeOffset)holidayEvent.Date).ToUnixTimeSeconds();

                    embed.AddField(eventName, $"{holidayEvent.Date:MMM dd, yyyy}\n <t:{unixTimestamp}:R>", false);

                }

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed).AddComponents(greenButton));
            }




            [SlashCommand("Ping", "Check the bot's connection")]
            public async Task Ping(InteractionContext ctx)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention} sent a ping! Was answered with pong"));

                Console.WriteLine($"{ctx.User.Mention} sent a ping request.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Sending PONG");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}


