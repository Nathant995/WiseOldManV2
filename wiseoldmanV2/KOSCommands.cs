using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

public class KOSCommands : ApplicationCommandModule
{
    [SlashCommand("kos", "View the KOS list.")]
    public async Task ViewKOS(InteractionContext ctx)
    {
        // Get the user's member information in the guild
        var member = await ctx.Guild.GetMemberAsync(ctx.User.Id);

        // Define the required role IDs
        ulong[] requiredRoleIds = { 715201066811260978, 715198934036774962, 1165345308042076235 };

        // Check if the user has any of the required roles
        if (!requiredRoleIds.Any(roleId => member.Roles.Any(role => role.Id == roleId)))
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("You don't have permission to use this command.")
                .AsEphemeral(true));
            return;
        }

        // Proceed with the command
        string filePath = "kos.txt"; // Path to your KOS list file
        string[] kosList = File.ReadAllLines(filePath);

        int rows = 10; // Number of rows
        int totalUsers = kosList.Length;

        // Calculate the number of columns needed
        int columns = (int)Math.Ceiling((double)totalUsers / rows);

        // Create an ephemeral embed with the KOS list in a spreadsheet-like format
        var embed = new DiscordEmbedBuilder
        {
            Title = "KOS List",
            Color = DiscordColor.Red, // Customize as needed
        };

        var separator = " | "; // Separator between cells

        // Format the list into a spreadsheet-like format
        var formattedList = new List<string>();

        for (int i = 0; i < rows; i++)
        {
            var row = new List<string>();

            for (int j = 0; j < columns; j++)
            {
                int index = i + j * rows;
                if (index < totalUsers)
                {
                    row.Add(kosList[index]);
                }
            }

            formattedList.Add(string.Join(separator, row));
        }

        embed.Description = string.Join("\n", formattedList);
        embed.Footer = new DiscordEmbedBuilder.EmbedFooter
        {
            Text = $"TOTAL KoS: {totalUsers}",
        };

        var builder = new DiscordInteractionResponseBuilder()
            .AddEmbed(embed)
            .AsEphemeral(true);

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
    }



    [SlashCommand("managekos", "Manage the KOS list.")]
    public async Task ManageKOS(InteractionContext ctx)
    {
        var member = await ctx.Guild.GetMemberAsync(ctx.User.Id);

        // Check if the user has the administrator permission
        if (!member.Permissions.HasPermission(Permissions.Administrator))
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("You don't have permission to use this command.")
                .AsEphemeral(true));
            return;
        }

        // Proceed with the command
        string filePath = "kos.txt"; // Path to your KOS list file
        string[] kosList = File.ReadAllLines(filePath);

        int usersPerColumn = 10;
        int totalUsers = kosList.Length;
        int totalColumns = (totalUsers + usersPerColumn - 1) / usersPerColumn; // Calculate total columns

        // Create an ephemeral embed with the KOS list in columns
        var embed = new DiscordEmbedBuilder
        {
            Title = "Manage KOS List",
            Color = DiscordColor.Red, // Customize as needed
        };

        var separator = " | "; // Separator between cells
        var formattedList = new List<string>();

        for (int col = 0; col < totalColumns; col++)
        {
            int startIndex = col * usersPerColumn;
            int endIndex = Math.Min(startIndex + usersPerColumn, totalUsers);
            var column = kosList[startIndex..endIndex];

            formattedList.Add(string.Join(separator, column));
        }

        embed.Description = string.Join("\n", formattedList);
        embed.Footer = new DiscordEmbedBuilder.EmbedFooter
        {
            Text = $"TOTAL KoS: {totalUsers}",
        };

        var addRemoveButtons = new DiscordButtonComponent[]
        {
        new DiscordButtonComponent(ButtonStyle.Success, "add_kos", "Add"),
        new DiscordButtonComponent(ButtonStyle.Danger, "remove_kos", "Remove"),
        };

        var builder = new DiscordInteractionResponseBuilder()
            .AddEmbed(embed)
            .AddComponents(addRemoveButtons)
            .AsEphemeral(true);

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
    }


}