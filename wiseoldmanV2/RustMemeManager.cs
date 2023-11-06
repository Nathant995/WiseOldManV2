using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Discord;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

namespace wiseoldmanV2
{
    public class RustMemeManager
    {
        private readonly DiscordClient _client;
        private readonly ulong _targetGuildId;
        private readonly ulong _targetChannelId;
        private DiscordChannel _targetChannel;
        private readonly string _cacheFilePath;
        private List<Tuple<string, DateTime>> _imageCache;
        private string _lastPostedImage;
        private System.Timers.Timer _cacheBuildingTimer;


        public RustMemeManager(DiscordClient client, ulong targetGuildId, ulong targetChannelId, string cacheFilePath)
        {
            _client = client;
            _targetGuildId = targetGuildId;
            _targetChannelId = targetChannelId;
            _cacheFilePath = cacheFilePath;
            _imageCache = new List<Tuple<string, DateTime>>();
            _targetChannel = client.GetChannelAsync(targetChannelId).Result;
            _client.MessageCreated += OnMessageCreated;
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            // Ignore messages from the bot itself
            if (e.Author.Id == client.CurrentUser.Id)
                return;

            // Check if the message is from the target channel and has attachments
            if (e.Channel.Id == _targetChannelId && e.Message.Attachments.Any())
            {
                foreach (var attachment in e.Message.Attachments)
                {
                    if (attachment.FileName.EndsWith(".jpg") || attachment.FileName.EndsWith(".png"))
                    {
                        _imageCache.Add(new Tuple<string, DateTime>(attachment.Url, e.Message.CreationTimestamp.DateTime));
                    }
                }
            }
        }

        public void StartCacheBuildingTimer()
        {
            // Set up a timer to periodically build the cache
            _cacheBuildingTimer = new System.Timers.Timer();
            _cacheBuildingTimer.Elapsed += CacheBuildingCallback;
            _cacheBuildingTimer.Interval = TimeSpan.FromHours(1).TotalMilliseconds; // Adjust the interval as needed
            _cacheBuildingTimer.AutoReset = true;
            _cacheBuildingTimer.Start();
        }

        public void BuildCacheOnStartup()
        {
            Console.WriteLine("[RustMemeManager] Initializing and updating cache at bot startup...");

            // Ensure that the cache file is created if it doesn't exist
            if (!File.Exists(_cacheFilePath))
            {
                File.Create(_cacheFilePath).Dispose();
            }

            // Load the existing cache or create an empty one
            LoadImageCache();

            // Call your cache-building logic on startup to populate the cache.
            var newMessages = GetMessagesWithAttachments(_targetChannel).Result;

            // Add the new messages to the cache
            foreach (var message in newMessages)
            {
                foreach (var attachment in message.Attachments)
                {
                    if (attachment.FileName.EndsWith(".jpg") || attachment.FileName.EndsWith(".png"))
                    {
                        _imageCache.Add(new Tuple<string, DateTime>(attachment.Url, message.CreationTimestamp.DateTime));
                    }
                }
            }

            // Save the cache
            SaveImageCache();
        }

        private void CacheBuildingCallback(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("Building cache over time...");

            // Ensure that the cache file is created if it doesn't exist
            if (!File.Exists(_cacheFilePath))
            {
                File.Create(_cacheFilePath).Dispose();
            }

            // Load the existing cache or create an empty one
            LoadImageCache();

            // Call your cache-building logic here, such as GetMessagesWithAttachments, to gradually populate the cache.
            var newMessages = GetMessagesWithAttachments(_targetChannel).Result;

            // Add the new messages to the cache
            foreach (var message in newMessages)
            {
                foreach (var attachment in message.Attachments)
                {
                    if (attachment.FileName.EndsWith(".jpg") || attachment.FileName.EndsWith(".png"))
                    {
                        _imageCache.Add(new Tuple<string, DateTime>(attachment.Url, message.CreationTimestamp.DateTime));
                    }
                }
            }

            // Save the cache
            SaveImageCache();
        }



        public void StopCacheBuildingTimer()
        {
            _cacheBuildingTimer.Stop();
            _cacheBuildingTimer.Dispose();
        }

        public async Task GetRandomRustMeme(InteractionContext ctx)
        {
            Console.WriteLine("Building cache...");
            // Check if the current guild matches the target guild
            var guild = ctx.Guild;

            if (guild == null || guild.Id != _targetGuildId)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command is not available in this guild."));
                return;
            }

            // Load the existing cache or create an empty one
            LoadImageCache();

            Console.WriteLine("Scanning channel...");
            var channel = guild.GetChannel(_targetChannelId);

            if (channel == null)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("The specified channel was not found."));
                return;
            }

            var messagesWithNewAttachments = await GetMessagesWithAttachments(channel);

            Console.WriteLine($"Scanning found {messagesWithNewAttachments.Count} new messages with attachments...");

            // Check if there are any messages with new attachments
            if (messagesWithNewAttachments.Count == 0)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("No new Rust memes found in the specified channel."));
                return;
            }

            Console.WriteLine($"Caching {messagesWithNewAttachments.Count} new image(s) to {_cacheFilePath}...");

            foreach (var message in messagesWithNewAttachments)
            {
                foreach (var attachment in message.Attachments)
                {
                    if (attachment.FileName.EndsWith(".jpg") || attachment.FileName.EndsWith(".png"))
                    {
                        _imageCache.Add(new Tuple<string, DateTime>(attachment.Url, message.CreationTimestamp.DateTime));
                    }
                }
            }

            Console.WriteLine("Saving cache...");
            // Save the updated cache
            SaveImageCache();

            Console.WriteLine("Cache data:");
            foreach (var cacheItem in _imageCache)
            {
                Console.WriteLine($"{cacheItem.Item1} - {cacheItem.Item2}");
            }

            Console.WriteLine($"Cache file size: {new FileInfo(_cacheFilePath).Length} bytes");
            Console.WriteLine($"Cache line count: {_imageCache.Count}");

            // Randomly select an image attachment from the updated cache
            var random = new Random();
            string randomAttachment;

            // Ensure the selected image is not the same as the last one
            do
            {
                var randomIndex = random.Next(_imageCache.Count);
                randomAttachment = _imageCache[randomIndex].Item1;
            } while (randomAttachment.Equals(_lastPostedImage));

            _lastPostedImage = randomAttachment;

            // Create an embed with the selected image
            var embed = new DiscordEmbedBuilder()
                .WithImageUrl(randomAttachment)
                .WithTitle("One from the Rust Archives...")
                .WithColor(DiscordColor.Gold) // Customize the color as needed
                .Build();

            // Send the embedded message to the current channel
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        private void LoadImageCache()
        {
            // Load the cache from the file if it exists, or create an empty list
            if (File.Exists(_cacheFilePath))
            {
                var lines = File.ReadAllLines(_cacheFilePath);
                _imageCache = new List<Tuple<string, DateTime>>();
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && DateTime.TryParse(parts[1], out var date))
                    {
                        _imageCache.Add(new Tuple<string, DateTime>(parts[0], date));
                    }
                }
            }
            else
            {
                _imageCache = new List<Tuple<string, DateTime>>();
            }
        }

        private void SaveImageCache()
        {
            // Save the cache to the file
            var lines = _imageCache.Select(item => $"{item.Item1},{item.Item2.ToString("O")}").ToArray();
            File.WriteAllLines(_cacheFilePath, lines);
        }

        private async Task<List<DiscordMessage>> GetMessagesWithAttachments(DiscordChannel channel)
        {
            var messages = new List<DiscordMessage>();
            ulong lastMessageId = 0;

            while (true)
            {
                var messagesBatch = await channel.GetMessagesAsync(limit: 100);

                if (messagesBatch.Count == 0)
                    break;

                // Filter messages to get only those after the last message
                messagesBatch = messagesBatch.Where(msg => msg.Id > lastMessageId).ToList();

                if (messagesBatch.Count == 0)
                    break;

                messages.AddRange(messagesBatch);
                lastMessageId = messagesBatch.Max(msg => msg.Id);

                // Sleep for a while to avoid rate limiting
                await Task.Delay(1000);
            }

            return messages;
        }
    }
}