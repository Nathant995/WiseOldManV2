﻿using System;
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
        private readonly Dictionary<ulong, ulong> messageChannelMap = new Dictionary<ulong, ulong>();



        public static InteractivityExtension Interactivity { get; private set; }


        // Property to expose the Discord client instance
        public DiscordClient Client
        {
            get { return _client; }
        }

        public async Task StartAsync()
        {
            try
            {
                await Task.Delay(2000);

                string token = "${KEY}"; //Remove before Uploa

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Starting bot with Token: " + token);
                _client = new DiscordClient(new DiscordConfiguration
                {
                    Token = token,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.All
                });

                _client.Ready += Client_ReadyAsync;
                _client.ComponentInteractionCreated += async (s, e) =>
                {
                    if (e.Id == "testbutton")
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

                        var followUpMessage = new DiscordMessageBuilder()
                            .WithContent("You clicked the button! :thumbsup:");

                        await e.Interaction.Channel.SendMessageAsync(followUpMessage);
                    }
                    else if (e.Id == "dismiss_button")
                    {
                        Console.WriteLine("[DEBUG] Dismiss button pressed for: " + e.Message.Id.ToString());

                        // Get the message ID from the interaction
                        var messageId = e.Message.Id;

                        // Get the channel ID from the interaction
                        var channelId = e.Channel.Id;

                        // Get the target channel
                        var targetChannel = await _client.GetChannelAsync(channelId);

                        // Get the message to delete
                        var messageToDelete = await targetChannel.GetMessageAsync(messageId);

                        if (messageToDelete != null)
                        {
                            // Ensure the bot has permission to delete messages
                            var botMember = await targetChannel.Guild.GetMemberAsync(_client.CurrentUser.Id);
                            if (botMember.PermissionsIn(targetChannel).HasPermission(Permissions.ManageMessages))
                            {
                                await messageToDelete.DeleteAsync();
                            }
                        }

                        // Send an ephemeral confirmation message to the user
                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .WithContent("The alert message has been dismissed.")
                            .AsEphemeral(true);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);
                    }
                    else if (e.Id == "add_kos")
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

                        // Send a prompt for the username
                        var promptMessage = new DiscordFollowupMessageBuilder()
                            .WithContent("Please type the username to add to the KOS list.")
                            .AsEphemeral(true);

                        var response = await e.Interaction.CreateFollowupMessageAsync(promptMessage);

                        // Get the username from the user's response
                        var interactivity = _client.GetInteractivity();
                        var nextMessageResult = await interactivity.WaitForMessageAsync(
                            x => x.Author.Id == e.User.Id, TimeSpan.FromMinutes(15));

                        if (nextMessageResult.TimedOut)
                        {
                            // Handle the case where the user didn't respond in time
                            var timeoutMessage = new DiscordFollowupMessageBuilder()
                                .WithContent("You took too long to respond")
                                .AsEphemeral(true);

                            await e.Interaction.CreateFollowupMessageAsync(timeoutMessage);
                        }
                        else
                        {
                            // Process the usernameToAdd here
                            var username = nextMessageResult.Result.Content.Trim();
                            var filePath = "kos.txt";

                            // Read the current KOS list
                            var kosList = File.ReadAllLines(filePath).ToList();

                            if (kosList.Contains(username))
                            {
                                // If the username already exists, send an error message
                                var errorMessage = new DiscordFollowupMessageBuilder()
                                    .WithContent("The username already exists in the KOS list.")
                                    .AsEphemeral(true);

                                await e.Interaction.CreateFollowupMessageAsync(errorMessage);
                            }
                            else
                            {
                                // Add the username to the KOS list
                                kosList.Add(username);
                                File.WriteAllLines(filePath, kosList);

                                // Send a confirmation message
                                var confirmationMessage = new DiscordFollowupMessageBuilder()
                                    .WithContent($"Added {username} to the KOS list.")
                                    .AsEphemeral(true);

                                await e.Interaction.CreateFollowupMessageAsync(confirmationMessage);

                                // Delete the user's name response
                                await nextMessageResult.Result.DeleteAsync();
                            }
                        }
                    }
                    else if (e.Id == "remove_kos")
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

                        // Send a prompt for the username to remove
                        var promptMessage = new DiscordFollowupMessageBuilder()
                            .WithContent("Please type the username to remove from the KOS list.")
                            .AsEphemeral(true);

                        var response = await e.Interaction.CreateFollowupMessageAsync(promptMessage);

                        // Get the username from the user's response
                        var interactivity = _client.GetInteractivity();
                        var nextMessageResult = await interactivity.WaitForMessageAsync(
                            x => x.Author.Id == e.User.Id, TimeSpan.FromMinutes(15));

                        if (nextMessageResult.TimedOut)
                        {
                            // Handle the case where the user didn't respond in time
                            var timeoutMessage = new DiscordFollowupMessageBuilder()
                                .WithContent("You took too long to respond")
                                .AsEphemeral(true);

                            await e.Interaction.CreateFollowupMessageAsync(timeoutMessage);
                        }
                        else
                        {
                            // Process the usernameToRemove here
                            var username = nextMessageResult.Result.Content.Trim();
                            var filePath = "kos.txt";

                            // Read the current KOS list
                            var kosList = File.ReadAllLines(filePath).ToList();

                            if (kosList.Contains(username))
                            {
                                // Remove the username from the KOS list
                                kosList.Remove(username);
                                File.WriteAllLines(filePath, kosList);

                                // Send a confirmation message
                                var confirmationMessage = new DiscordFollowupMessageBuilder()
                                    .WithContent($"Removed {username} from the KOS list.")
                                    .AsEphemeral(true);

                                await e.Interaction.CreateFollowupMessageAsync(confirmationMessage);

                                // Delete the user's name response
                                await nextMessageResult.Result.DeleteAsync();
                            }
                            else
                            {
                                // If the username doesn't exist, send an error message
                                var errorMessage = new DiscordFollowupMessageBuilder()
                                    .WithContent("The username does not exist in the KOS list.")
                                    .AsEphemeral(true);

                                await e.Interaction.CreateFollowupMessageAsync(errorMessage);
                            }
                        }
                    }
                };




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
                _healthCheckTimer = new System.Timers.Timer(900000); // 15 minutes in milliseconds
                _healthCheckTimer.Elapsed += HealthCheckTimer_Elapsed;
                _healthCheckTimer.AutoReset = true;
                _healthCheckTimer.Start();



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
                // Disconnect the bot from Discord
                await _client.DisconnectAsync();

                // Dispose of the bot client to release resources
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

        public class UtilitySlashCommands : ApplicationCommandModule // Inherit from ApplicationCommandModule
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
                //  await Task.Delay(1000);
                await ctx.Channel.DeleteMessagesAsync(messages);
                //await Task.Delay(300);
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

            [SlashCommand("source", "View Wise Old Man's Open Sourcecode")]
            public async Task SourceCodeCommand(InteractionContext ctx)
            {
                // GitHub repository URL
                string githubRepoUrl = "https://api.github.com/repos/Nathant995/WiseOldManV2";

                // Bot avatar URL
                string botAvatarUrl = ctx.Client.CurrentUser.AvatarUrl;

                // Fetch additional information from the GitHub API
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "WiseOldMan");

                HttpResponseMessage response = await httpClient.GetAsync(githubRepoUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response to extract repository information
                    var repoInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);
                    string repoDescription = repoInfo.description;
                    int starsCount = repoInfo.stargazers_count;

                    // Get the total number of commits for the repository
                    HttpResponseMessage commitResponse = await httpClient.GetAsync(githubRepoUrl + "/commits");
                    var commitInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await commitResponse.Content.ReadAsStringAsync());
                    int commitCount = commitInfo.Count;

                    // Get the total number of releases for the repository
                    HttpResponseMessage releaseResponse = await httpClient.GetAsync(githubRepoUrl + "/releases");
                    var releaseInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await releaseResponse.Content.ReadAsStringAsync());
                    int releaseCount = releaseInfo.Count;

                    // Create an embed with information
                    var embedBuilder = new DiscordEmbedBuilder
                    {
                        Title = "Under The Hood",
                        Description = $"The Wise Old Man's Source Code can be found [here](https://github.com/Nathant995/WiseOldManV2).",
                        Color = new DiscordColor(0x7289DA), 
                        ImageUrl = botAvatarUrl, 
                        Url = githubRepoUrl, 
                    };

                    // Add additional information to the embed
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

                        // Log to console
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"[ALERT] Command executed by {ctx.Member.Username} in guild {ctx.Guild.Name}, channel {textChannel.Name} at {DateTime.Now} - Message sent successfully.");
                    }
                    else
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("Oops, I was unable to send that report! Try again later."));

                        // Log to console
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"[ALERT] Command executed by {ctx.Member.Username} in guild {ctx.Guild.Name} at {DateTime.Now} - Could not find the specified channel, does the bot have permissions \n Maybe the channel does not exist?");
                    }
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Oops, I was unable to send that report! I'm having trouble connecting to the guild. Try again later."));

                    // Log to console
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[ALERT] Command executed by {ctx.Member.Username} at {DateTime.Now} - Could not find the specified guild. (Check if the bot is part of W00ters Rank Hub)");
                }

                // Reset console text color to white
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

