using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public RustMemeManager(DiscordClient client, ulong targetGuildId, ulong targetChannelId)
        {
            _client = client;
            _targetGuildId = targetGuildId;
            _targetChannelId = targetChannelId;
        }

        public async Task GetRandomRustMeme(InteractionContext ctx)
        {
            // Check if the current guild matches the target guild
            if (ctx.Guild == null || ctx.Guild.Id != _targetGuildId)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command is not available in this guild."));
                return;
            }

            // Get the specified channel in the target guild
            var channel = ctx.Guild.Channels[_targetChannelId];

            // Get the messages in the specified channel
            var messages = await channel.GetMessagesAsync();

            // Filter messages to include only those with attachments
            var messagesWithAttachments = messages.Where(msg => msg.Attachments.Any()).ToList();

            // Check if there are any messages with attachments
            if (messagesWithAttachments.Count == 0)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("No Rust memes found in the specified channel."));
                return;
            }

            // Create a list to store all image attachments
            var imageAttachments = new List<DiscordAttachment>();

            // Extract all image attachments from the messages
            foreach (var message in messagesWithAttachments)
            {
                imageAttachments.AddRange(message.Attachments.Where(attachment => attachment.FileName.EndsWith(".jpg") || attachment.FileName.EndsWith(".png")));
            }

            // Check if there are any image attachments
            if (imageAttachments.Count == 0)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("No Rust memes found in the specified channel."));
                return;
            }

            // Randomly select an image attachment
            var random = new Random();
            var randomAttachment = imageAttachments[random.Next(imageAttachments.Count)];

            // Create an embed with the selected image
            var embed = new DiscordEmbedBuilder()
                .WithImageUrl(randomAttachment.Url)
                .WithTitle("One from the Rust Archives...")
                .WithColor(DiscordColor.Gold) // Customize the color as needed
                .Build();

            // Send the embedded message to the current channel
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }
    }
}
