
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.EventArgs;



namespace wiseoldmanV2
{
    public class ButtonManager
    {
        private readonly DiscordClient _client;

        public ButtonManager(DiscordClient client)
        {
            _client = client;
        }

        public void Initialize()
        {
            _client.ComponentInteractionCreated += async (s, e) =>
            {
                if (e.Id == "testbutton")
                {
                    await HandleTestButton(e);
                }
                else if (e.Id == "dismiss_button")
                {
                    await HandleDismissButton(e);
                }
                else if (e.Id == "add_kos")
                {
                    await HandleAddKOSButton(e);
                }
                else if (e.Id == "remove_kos")
                {
                    await HandleRemoveKOSButton(e);
                }
                // Add more button handling logic here be sure to create an async task respectively
            };
        }

        private async Task HandleTestButton(ComponentInteractionCreateEventArgs e)
        {
            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

            var followUpMessage = new DiscordMessageBuilder()
                .WithContent("You clicked the button! :thumbsup:");

            await e.Interaction.Channel.SendMessageAsync(followUpMessage);
        }

        private async Task HandleDismissButton(ComponentInteractionCreateEventArgs e)
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

        private async Task HandleAddKOSButton(ComponentInteractionCreateEventArgs e)
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

        private async Task HandleRemoveKOSButton(ComponentInteractionCreateEventArgs e)
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

                var kosList = File.ReadAllLines(filePath).ToList();

                if (kosList.Contains(username))
                {
                    kosList.Remove(username);
                    File.WriteAllLines(filePath, kosList);

                    var confirmationMessage = new DiscordFollowupMessageBuilder()
                        .WithContent($"Removed {username} from the KOS list.")
                        .AsEphemeral(true);

                    await e.Interaction.CreateFollowupMessageAsync(confirmationMessage);

                    await nextMessageResult.Result.DeleteAsync();
                }
                else
                {
                    var errorMessage = new DiscordFollowupMessageBuilder()
                        .WithContent("The username does not exist in the KOS list.")
                        .AsEphemeral(true);

                    await e.Interaction.CreateFollowupMessageAsync(errorMessage);
                }
            }
        }
    }
}