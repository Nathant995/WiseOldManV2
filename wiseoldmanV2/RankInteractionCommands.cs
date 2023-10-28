using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace wiseoldmanV2
{
    public class RankInteractionCommands : ApplicationCommandModule
    {
        private InteractivityExtension _interactivity;
        // Define a dictionary to store MessageID and ChannelID
        private readonly Dictionary<ulong, ulong> messageChannelMap = new Dictionary<ulong, ulong>();

        #region Interaction Commands (Buttons & Dropdown Menus) TEST CASE BUTTONS

        [SlashCommand("BetterButtonTest", "Nath's Attempt at better buttons")]
        public async Task BetterButton(InteractionContext ctx)
        {

            var member = await ctx.Guild.GetMemberAsync(ctx.User.Id);
            

            // Check if the user has the administrator permission
            if (!member.PermissionsIn(ctx.Channel).HasPermission(Permissions.Administrator))
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You don't have permission to use this command.").AsEphemeral(true));
                return;
            }

            //Buttons
            var myButton = new DiscordButtonComponent(ButtonStyle.Primary, "testbutton", "Default!");
            var greyButton = new DiscordButtonComponent(ButtonStyle.Secondary, "2_top_d", "Grey!");
            var greenButton = new DiscordButtonComponent(ButtonStyle.Success, "3_top_d", "Green!");
            var redButton = new DiscordButtonComponent(ButtonStyle.Danger, "4_top_d", "Red!");
            var linkButton = new DiscordLinkButtonComponent("https://www.runescape.com", "Link!");


            //Message builder
            var builder = new DiscordInteractionResponseBuilder()
                .WithContent("This message has buttons! Pretty neat ennit!")
                .AddComponents(myButton, greyButton, greenButton, redButton, linkButton)
                .AsEphemeral(true);


            //Response
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }

        [SlashCommand("testAlert", "Send an alert message to a channel")]
        public async Task TestAlert(InteractionContext ctx, [Option("channel", "The channel to send the alert to")] string channelId)
        {
            if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.Administrator))
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You don't have permission to use this command.").AsEphemeral(true));
                return;
            }

            if (ulong.TryParse(channelId, out ulong channelIdValue))
            {
                var targetChannel = await ctx.Client.GetChannelAsync(channelIdValue);

                if (targetChannel is DiscordChannel textChannel)
                {
                    var builder = new DiscordMessageBuilder()
                        .WithContent("This is an alert message.")
                        .AddComponents(new DiscordButtonComponent(ButtonStyle.Success, "dismiss_button", "Dismiss"));

                    var message = await targetChannel.SendMessageAsync(builder);

                    if (message != null)
                    {
                        // Store the mapping of message ID to channel ID
                        messageChannelMap[message.Id] = targetChannel.Id;

                        // Here, you can also store the message ID in a collection or dictionary to reference it later for deletion
                    }
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Invalid channel specified.").AsEphemeral(true));
                }
            }
            else
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Invalid channel ID.").AsEphemeral(true));
            }
        }




        [SlashCommand("testBB", "Button test - Don't Use")]
        public async Task TestButtonCommand(InteractionContext ctx) { 
       // Create a button component
            var button = new DiscordButtonComponent(ButtonStyle.Primary, "my_button", "Click me!");

        // Create an interaction response builder with the button
        var responseBuilder = new DiscordInteractionResponseBuilder()
            .WithContent("Click this button:")
            .AddComponents(button);

        // Respond with the button
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);

        // Fetch the message manually
        var message = await ctx.Channel.GetMessageAsync(ctx.Interaction.Id);

        // Wait for a button interaction
        var interactivity = ctx.Client.GetInteractivity();
        var buttonResult = await interactivity.WaitForButtonAsync(message, "my_button", TimeSpan.FromMinutes(1));

            // Handle the button interaction
            if (buttonResult.TimedOut)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("You didn't click the button in time!"));
            }
            else
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("You clicked the button!"));
            }
        }

        



        [SlashCommand("testDD", "DropDown Test - Don't Use")]
        public async Task TestCommand(InteractionContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption("Label, no description", "label_no_desc"),
                new DiscordSelectComponentOption("Label, Description", "label_with_desc", "This is a description!"),
                new DiscordSelectComponentOption("Label, Description, Emoji", "label_with_desc_emoji", "This is a description!", emoji: new DiscordComponentEmoji(854260064906117121)),
                new DiscordSelectComponentOption("Label, Description, Emoji (Default)", "label_with_desc_emoji_default", "This is a description!", isDefault: true, emoji: new DiscordComponentEmoji(854260064906117121))
            };

            var dropdown = new DiscordSelectComponent("dropdown", "Select an option", options, false, 1, 2);

            var responseBuilder = new DiscordInteractionResponseBuilder()
                .WithContent("Look, it's a dropdown!")
                .AddComponents(dropdown);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);
        }

        #endregion

    }
}