using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

public class HolidayEvent
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public Func<DiscordEmbedBuilder> MessageBuilder { get; set; }
    public bool HasPosted { get; set; } = false;

}

public class HolidayEventManager
{
    private readonly DiscordClient _client;
    private readonly List<HolidayEvent> _events;

    private readonly Dictionary<ulong, ulong> _eventChannels = new Dictionary<ulong, ulong>
    {
        // Add your guild and channel pairs here
        { 686840457334227129, 686840457338421251 },
        { 715197288984870932, 716355312449355786 }
    };

    public HolidayEventManager(DiscordClient client)
    {

        _client = client;
        _events = new List<HolidayEvent>();

        // Subscribe to the ClientReady event when the bot is ready
        _client.Ready += ClientReadyAsync;

        // Add Halloween event
        AddHolidayEvent("Halloween", new DateTime(DateTime.Now.Year, 10, 31, 0, 0, 0), () => new DiscordEmbedBuilder
        {
            Title = "🎃 Happy Halloween! 🎃",
            Description = "Boo! 👻 It's time for tricks and treats! Wishing you a frightfully fun Halloween filled with magical surprises! May you get lots of treats that are good to eat!",
            Color = new DiscordColor(0xFF4500), // Orange color
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "👻🍬 Stay spooky and have a great Halloween! 🍬👻"
            }
        });

        // Add Christmas event
        AddHolidayEvent("Christmas", new DateTime(DateTime.Now.Year, 12, 25, 0, 0, 0), () => new DiscordEmbedBuilder
        {
            Title = "🎄 Merry Christmas! 🎄",
            Description = "Ho Ho Ho! 🎅 May your Christmas sparkle with moments of love, laughter and goodwill. And may the year ahead be full of contentment and joy. Have a Merry Christmas and we look forward to seeing you in the New Year.",
            Color = new DiscordColor(0xFF0000), // Red color
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "🎁🌟🎄 May your Christmas be merry and bright! 🎄🌟🎁"
            }
        });

        // Add New Year's Day event
        AddHolidayEvent("New Year's Day", new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0), () => new DiscordEmbedBuilder
        {
            Title = "🎉 Happy New Year! 🎉",
            Description = "🥂 Cheers to the New Year! 🥂 Wishing you a Happy New Year with the hope that you will have many blessings in the year to come. Let's toast to yesterday’s achievements and tomorrow’s bright future!",
            Color = new DiscordColor(0xFFD700), // Gold color
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "🍾💫 Here's to a bright New Year and a fond farewell to the old! 💫🍾"
            }
        });
    }


    public void AddEvent(HolidayEvent holidayEvent)
    {
        _events.Add(holidayEvent);
    }

    public void AddHolidayEvent(string name, DateTime date, Func<DiscordEmbedBuilder> messageBuilder)
    {
        var holidayEvent = new HolidayEvent
        {
            Name = name,
            Date = date,
            MessageBuilder = messageBuilder
        };

        AddEvent(holidayEvent);
    }

    public IReadOnlyList<HolidayEvent> GetUpcomingEvents()
    {
        var now = DateTime.Now;
        return _events.Where(e => e.Date > now && !e.HasPosted).ToList();
    }
    private async Task ClientReadyAsync(DiscordClient sender, ReadyEventArgs e)
    {
        await Task.Delay(3000); // Delay to ensure the bot is fully connected

        // Create a daily timer that checks for upcoming events
        var dailyTimer = new System.Timers.Timer(24 * 60 * 60 * 1000); // 24 hours in milliseconds
        dailyTimer.Elapsed += async (_, _) => await CheckForUpcomingEvents();
        dailyTimer.Start();

        // Immediately check for upcoming events
        await CheckForUpcomingEvents();
    }

    private async Task CheckForUpcomingEvents()
    {
        foreach (var holidayEvent in _events)
        {
            var now = DateTime.Now;
            var timeUntilEvent = holidayEvent.Date - now;

            if (timeUntilEvent.TotalMilliseconds <= 0 || holidayEvent.HasPosted)
            {
                // Event time has passed or has already been posted, skip this event
                continue;
            }

            // Format time until the event
            string timeUntilEventStr;
            if (timeUntilEvent.TotalHours < 1)
            {
                timeUntilEventStr = "Any Moment";
            }
            else
            {
                int months = timeUntilEvent.Days / 30; // Approximate months for simplicity
                int weeks = (timeUntilEvent.Days % 30) / 7;
                int days = timeUntilEvent.Days % 7;
                int hours = timeUntilEvent.Hours;

                timeUntilEventStr = $"{months} Months, {weeks} Weeks, {days} Days, {hours} Hours";
            }

            // Output time until the event and its name
            Console.WriteLine($"Time until {holidayEvent.Name}: {timeUntilEventStr}");

            if (timeUntilEvent.TotalMilliseconds <= 24 * 60 * 60 * 1000) // If event is within the next 24 hours
            {
                // Calculate the timer interval in milliseconds as a long
                long timerInterval = (long)timeUntilEvent.TotalMilliseconds;

                // Schedule sending the event message for each guild and channel
                foreach (var (guildId, channelId) in _eventChannels)
                {
                    var timer = new System.Timers.Timer(timerInterval);
                    timer.Elapsed += async (_, _) =>
                    {
                        // Get the channel to send the message to
                        var guild = await _client.GetGuildAsync(guildId);
                        var channel = guild?.GetChannel(channelId);

                        if (channel != null)
                        {
                            // Build and send the event message
                            var embed = holidayEvent.MessageBuilder();
                            await channel.SendMessageAsync(embed: embed);

                            // Output confirmation that the event was posted
                            Console.WriteLine($"Posted {holidayEvent.Name} event in {channel.Name}.");

                            // Mark the event as posted
                            holidayEvent.HasPosted = true;
                        }
                    };
                    timer.Start();
                }
            }
        }
    }
}
