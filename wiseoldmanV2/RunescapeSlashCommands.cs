using System;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Globalization;
using DSharpPlus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace wiseoldmanV2
{
    public class RuneScapeSlashCommands : ApplicationCommandModule
    {



        private static readonly HttpClient _httpClient = new HttpClient();

        #region GE
        //  [SlashCommand("geinfo", "Get information about a RuneScape item.")]
        //  public async Task GeInfoCommand(InteractionContext ctx, [Option("item", "The name of the item.")] string item)
        //  {
        //    try
        //  {
        //    // Fetch the JSON data from the provided URL
        //   var httpClient = new HttpClient();
        //  var jsonData = await httpClient.GetStringAsync("https://runescape.wiki/?title=Module:GEIDs/data.json&action=raw&ctype=application%2Fjson");

        // Parse the JSON data
        //  var data = JsonConvert.DeserializeObject<JObject>(jsonData);

        // Try to find the item ID by name (case-insensitive)
        //                var itemId = FindItemIdByName(data, item);

        //              if (itemId.HasValue)
        //            {
        //              var geManager = new GeManager();
        //            var itemInfoEmbedBuilder = await geManager.GetItemInfoByIdAsync(itemId.Value);

        // Fetch and add the price graph data to the embed
        //          var priceGraphUrl = await geManager.GetItemPriceGraphUrlAsync(itemId.Value);

        // Add the field to the embed
        //        itemInfoEmbedBuilder.AddField("Price Trend Graph", priceGraphUrl, true);

        //    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(itemInfoEmbedBuilder.Build()));
        //  }
        //   else
        // {
        // Item not found in the JSON data
        //   await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Item not found."));
        //   }
        // }
        //  catch (Exception ex)
        //  {
        // Handle any errors
        //    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"An error occurred: {ex.Message}"));
        // }
        // }

        // private int? FindItemIdByName(JObject data, string itemName)
        // {
        //   foreach (var property in data.Properties())
        //   {
        //     if (string.Equals(property.Name, itemName, StringComparison.OrdinalIgnoreCase))
        //   {
        //     return property.Value.Value<int>();
        //   }
        // }
        //  return null; // Item not found
        // }

        #endregion

        [SlashCommand("rustmeme", "Get a random Rust meme from the archives.")]
        public async Task RustMemeCommand(InteractionContext ctx)
        {
            // Create an instance of RustMemeManager with the cache file path and call the method to get a random Rust meme
            ulong targetGuildId = 715197288984870932; //w00ters rank hub
            ulong targetChannelId = 718127567273721897; //Channel to scan for Rust Memes
            string cacheFilePath = "rustmeme.cache"; // Replace with the actual path
            var memeManager = new RustMemeManager(ctx.Client, targetGuildId, targetChannelId, cacheFilePath);
            await memeManager.GetRandomRustMeme(ctx);
        }



        [SlashCommand("markdonalds", "Get that sweet MarkyDs")]
        public async Task GetMarkDonalds(InteractionContext ctx)
        {
            string imageUrl = "https://media.licdn.com/dms/image/C5112AQH9l7O8kGRg6A/article-cover_image-shrink_600_2000/0/1566811009760?e=2147483647&v=beta&t=tg7wx2s2HvMahkHceZI-RITIaEquAyIswd2WqF7qpE4";

            var embed = new DiscordEmbedBuilder
            {
                Title = "Welcome to MarkDonalds can I take ur order.",
                ImageUrl = imageUrl,
                Color = new DiscordColor(144, 238, 144) // Blue color
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .AddEmbed(embed)
                .AsEphemeral(true)
            );
        }

        [SlashCommand("needjesus", "Summon the power of Jesus to the chat")]
        public async Task NeedJesus(InteractionContext ctx)
        {
            string gifUrl = "https://media.giphy.com/media/3og0IT4reEVvqugJZC/giphy.gif";

            var embed = new DiscordEmbedBuilder
            {
                ImageUrl = gifUrl
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent($"Summoning the power of Jesus! :pray:")
                .AddEmbed(embed));

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{ctx.User.Username} summoned the power of Jesus!");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        [SlashCommand("joke", "Tell a RuneScape-themed joke")]
        public async Task TellJoke(InteractionContext ctx)
        {
            string[] jokes = new string[]
            {
                  "Why did the RuneScape player refuse to fight dragons anymore? Because they were tired of \"dragon\" their problems along!",
                  "What's a wizard's favorite type of music in RuneScape? \"Rock and Rune\"!",
                  "Why did the adventurer bring a ladder to the RuneScape party? Because they heard the party was in the \"high scores\"!",
                  "What do you call a player who only plays RuneScape during winter? A \"snow-verachiever\"!",
                  "Why did the pirate in RuneScape fail to pass their sailing exam? Because they couldn't \"sea\" the answers!",
                  "How do you make a goblin laugh in RuneScape? Tell them a \"goblin-to-goblin\" joke!",
                  "What's a RuneScape millionaire's favorite type of sandwich? A \"gilded cheese\" sandwich, of course!",
                  "How does a RuneScape noob find their way around? They follow the \"arrow-dotted\" line on their map!",
                  "Why did the RuneScape chef start a stand-up comedy career? Because they realized they could \"dish\" out jokes as well as food!",
                  "What's a fish's favorite instrument in RuneScape? The \"bass\" guitar!",
                  "Why did the W00ters clan always excel at hide and seek in RuneScape? Because they had the best \"Mod-spotting\" skills!",
                  "How does a W00ters member greet their friends? They say, \"W00t's up!\"",
                  "Why did the W00ters clan start a gardening club in RuneScape? Because they wanted to \"grow\" their friendship!",
                  "How do W00ters members stay cool in the heat of battle? They bring along their \"W00t-er\" bottles!",
                  "What's a W00ters member's favorite type of tree in RuneScape? The \"W00dcutting\" tree, of course!",
                  "How did the W00ters clan celebrate their anniversary in RuneScape? They threw a \"W00t-astic\" party!",
                  "Why did the W00ters clan excel in agility training? Because they were always in a \"W00t-like\" sprint!",
                  "What's a W00ters member's favorite type of food in RuneScape? \"Markdonalds!\"",
                  "How do W00ters members stay motivated during quests? They chant, \"W00t for success!\"",
                  "Why did the W00ters clan recruit a chef in RuneScape? Because Markdonalds is craving \"W00t-licious\" meals!"
            };

            Random rand = new Random();
            int randomIndex = rand.Next(jokes.Length);
            string selectedJoke = jokes[randomIndex];

            var embed = new DiscordEmbedBuilder
            {
                Title = "RuneScape Joke",
                Description = selectedJoke,
                Color = new DiscordColor(0, 255, 255) // Cyan color
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .AddEmbed(embed)
            );
        }

        [SlashCommand("Metrics", "Retrieve a player's RuneMetrics profile data (RS3)")]
        public async Task GetMetrics(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                // Replace spaces with URL-encoded spaces in the player's name for the URL
                string urlPlayerName = Uri.EscapeDataString(playerName);

                string url = $"https://apps.runescape.com/runemetrics/profile/profile?user={urlPlayerName}&activities=20";
                string response = await _httpClient.GetStringAsync(url);

                var metricsData = JsonConvert.DeserializeObject<RuneMetricsProfile>(response);

                // Fetch the user's avatar
                string avatarUrl = $"http://secure.runescape.com/m=avatar-rs/{urlPlayerName}/chat.png";

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{metricsData.Name}'s RuneMetrics Profile",
                    Color = new DiscordColor(252, 185, 0),
                };

                // Format large numbers with commas
                embed.AddField("**__Overall Rank__**", metricsData.Rank, true);
                embed.AddField("**__Total Skill Level__**", metricsData.TotalSkill.ToString(), true);
                embed.AddField("**__Total Experience__**", metricsData.TotalXP.ToString("N0"), true);
                embed.AddField("**__Combat Level__**", metricsData.CombatLevel.ToString(), true);
                embed.AddField("**__Magic Experience__**", metricsData.Magic.ToString("N0"), true);
                embed.AddField("**__Melee Experience__**", metricsData.Melee.ToString("N0"), true);
                embed.AddField("**__Ranged Experience__**", metricsData.Ranged.ToString("N0"), true);
                embed.AddField("**__Quests Started__**", metricsData.QuestsStarted.ToString(), true);
                embed.AddField("**__Quests Completed__**", metricsData.QuestsComplete.ToString(), true);
                embed.AddField("**__Quests Not Started__**", metricsData.QuestsNotStarted.ToString(), true);
                embed.AddField("**__Online Status__**", metricsData.LoggedIn == "true" ? "Online" : "Offline");

                var activities = string.Join("\n", metricsData.Activities.Select(a => a.Text));
                embed.AddField("**__Recent Activities__**", activities);
                embed.WithThumbnail(avatarUrl);

                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                embed.WithFooter($"Metric data timestamp: {timestamp}", null);

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, here's the RuneMetrics profile data for **{playerName}**:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneMetrics data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the RuneMetrics profile data. Please try again later.")
                );
            }
        }




        [SlashCommand("osstats", "Search Old School RuneScape hiscores for a player's stats")]
        public async Task GetOSStats(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                // Replace spaces with underscores in the player's name for the URL
                string urlPlayerName = playerName.Replace(" ", "_");
                string url = $"https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws?player={urlPlayerName}";
                string response = await _httpClient.GetStringAsync(url);
                string[] stats = response.Split('\n');

                var table = new StringBuilder();
                table.AppendLine("Skill           Rank        Level     XP   ");
                table.AppendLine("_______________________________________\n");

                for (int i = 0; i < stats.Length; i++)
                {
                    var stat = stats[i].Split(',');

                    if (stat.Length >= 3)
                    {
                        var rank = stat[0];
                        var skillName = GetSkillName(i);
                        var level = stat[1];
                        var experience = stat[2];

                        string formattedSkill = skillName.PadRight(14); // Adjust the padding as needed
                        string formattedRank = FormatRank(rank).PadRight(14); // Adjust the padding as needed
                        string formattedLevel = level.PadRight(8); // Adjust the padding as needed
                        string formattedXP = FormatXP(experience).PadRight(20); // Adjust the padding as needed

                        table.AppendLine($"{formattedSkill}{formattedRank}{formattedLevel}{formattedXP}");
                    }
                }

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{playerName}'s OSRS Stats",
                    Color = new DiscordColor(252, 185, 0), // Gold color
                    Description = "```" + table.ToString() + "```"
                };

                string osrsLink = $"[View Full OSRS Stats](https://secure.runescape.com/m=hiscore_oldschool/a=12/compare?user1={urlPlayerName})";
                embed.AddField("Full Stats", osrsLink);
                embed.WithFooter("*Note: Data may be slightly inaccurate due to refresh times.");

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, here's the Old School RuneScape high scores for {playerName}:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneScape data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the stats. Please try again later.")
                );
            }
        }



        [SlashCommand("RS3stats", "Search RuneScape hiscores for a player's stats (RS3)")]
        public async Task GetStats(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                // Replace spaces with underscores in the player's name for the URL
                string urlPlayerName = playerName.Replace(" ", "_");
                string url = $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={urlPlayerName}";
                string response = await _httpClient.GetStringAsync(url);
                string[] stats = response.Split('\n');

                var table = new StringBuilder();
                table.AppendLine("Skill           Rank        Level     XP   ");
                table.AppendLine("___________________________________________\n");

                for (int i = 0; i < stats.Length; i++)
                {
                    var stat = stats[i].Split(',');

                    if (stat.Length >= 3)
                    {
                        var skillName = GetSkillName(i).Replace("_", " "); // Replace underscores with spaces
                        var level = stat[1];
                        var experience = stat[2];

                        string formattedSkill = skillName.PadRight(14); // Adjust the padding as needed
                        string formattedRank = FormatRank(stat[0]).PadRight(14); // Adjust the padding as needed
                        string formattedLevel = level.PadRight(8); // Adjust the padding as needed
                        string formattedXP = FormatXP(experience).PadRight(20); // Adjust the padding as needed

                        table.AppendLine($"{formattedSkill}{formattedRank}{formattedLevel}{formattedXP}");
                    }
                }

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{playerName}'s RS3 Stats",
                    Color = new DiscordColor(252, 185, 0), // Gold color
                    Description = "```" + table.ToString() + "```"
                };

                string rsLink = $"[View Full Stats](https://secure.runescape.com/m=hiscore/a=12/compare?user1={urlPlayerName})";
                embed.AddField("Full Stats", rsLink);
                embed.WithFooter("*Note: Data may be slightly inaccurate due to Refresh times.");

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, here's the highscores stats for {playerName}:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneScape data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the stats. Please try again later.")
                );
            }
        }

        [SlashCommand("RS3_iron_stats", "Search RuneScape 3 Ironman hiscores for a player's stats")]
        public async Task GetIronStats(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                string url = $"https://secure.runescape.com/m=hiscore_ironman/index_lite.ws?player={playerName}";
                string response = await _httpClient.GetStringAsync(url);
                string[] stats = response.Split('\n');

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{playerName}'s RS3 Ironman Stats",
                    Color = new DiscordColor(252, 185, 0), // Gold color
                };

                string summary = "";

                for (int i = 0; i < stats.Length; i++)
                {
                    var stat = stats[i].Split(',');

                    if (stat.Length >= 3)
                    {
                        var skillName = GetSkillName(i).Replace("_", " "); // Replace underscores with spaces
                        var rank = FormatRank(stat[0]).PadRight(10); // Adjust the padding as needed
                        var level = stat[1].PadRight(8); // Adjust the padding as needed
                        var experience = FormatXP(stat[2]).PadRight(20); // Adjust the padding as needed

                        summary += $"**{skillName}:** Rank {rank}, Level {level}, Exp {experience}\n";
                    }
                }

                embed.Description = summary;
                string ironLink = $"[View Full Ironman Stats](https://secure.runescape.com/m=hiscore/a=12/compare?user1={playerName})";
                embed.AddField("Full Stats", ironLink);
                embed.WithFooter("*Note: Data may be slightly inaccurate due to Refresh times.");

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, here's the RuneScape 3 Ironman high scores for {playerName}:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneScape data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the stats. Please try again later.")
                );
            }
        }

        [SlashCommand("RS3_hciron_stats", "Search RuneScape 3 Hardcore Ironman hiscores for a player's stats")]
        public async Task GetHCIronStats(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                string url = $"https://secure.runescape.com/m=hiscore_hardcore_ironman/index_lite.ws?player={playerName}";
                string response = await _httpClient.GetStringAsync(url);
                string[] stats = response.Split('\n');

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{playerName}'s RS3 Hardcore Ironman Stats",
                    Color = new DiscordColor(252, 185, 0),
                };

                string summary = "";

                for (int i = 0; i < stats.Length; i++)
                {
                    var stat = stats[i].Split(',');

                    if (stat.Length >= 3)
                    {
                        var skillName = GetSkillName(i).Replace("_", " ");
                        var rank = FormatRank(stat[0]).PadRight(10);
                        var level = stat[1].PadRight(8);
                        var experience = FormatXP(stat[2]).PadRight(20);

                        summary += $"**{skillName}:** Rank {rank}, Level {level}, Exp {experience}\n";
                    }
                }

                embed.Description = summary;
                string hcIronLink = $"[View Full Hardcore Ironman Stats](https://secure.runescape.com/m=hiscore_hardcore/a=12/compare?user1={playerName})";
                embed.AddField("Full Stats", hcIronLink);
                embed.WithFooter("*Note: Data may be slightly inaccurate due to Refresh times.");

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, here's the RuneScape 3 Hardcore Ironman high scores for {playerName}:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneScape data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the stats. Please try again later.")
                );
            }
        }

        [SlashCommand("os_iron_stats", "Search Old School RuneScape Ironman hiscores for a player's stats")]
        public async Task GetOSIronStats(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                // Replace spaces with underscores in the player's name for the URL
                string urlPlayerName = playerName.Replace(" ", "_");
                string url = $"https://secure.runescape.com/m=hiscore_oldschool_ironman/index_lite.ws?player={urlPlayerName}";
                string response = await _httpClient.GetStringAsync(url);
                string[] stats = response.Split('\n');

                var table = new StringBuilder();
                table.AppendLine("Skill           Rank        Level     XP   ");
                table.AppendLine("_______________________________________\n");

                for (int i = 0; i < stats.Length; i++)
                {
                    var stat = stats[i].Split(',');

                    if (stat.Length >= 3)
                    {
                        var rank = stat[0];
                        var skillName = GetSkillName(i).Replace("_", " "); // Replace underscores with spaces
                        var level = stat[1];
                        var experience = stat[2];

                        string formattedSkill = skillName.PadRight(14); // Adjust the padding as needed
                        string formattedRank = FormatRank(rank).PadRight(14); // Adjust the padding as needed
                        string formattedLevel = level.PadRight(8); // Adjust the padding as needed
                        string formattedXP = FormatXP(experience).PadRight(20); // Adjust the padding as needed

                        table.AppendLine($"{formattedSkill}{formattedRank}{formattedLevel}{formattedXP}");
                    }
                }

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{playerName}'s OS Ironman Stats",
                    Color = new DiscordColor(252, 185, 0), // Gold color
                    Description = "```" + table.ToString() + "```"
                };

                string osIronLink = $"[View Full OS Ironman Stats](https://secure.runescape.com/m=hiscore_oldschool_ironman/a=12/compare?user1={urlPlayerName})";
                embed.AddField("Full Stats", osIronLink);
                embed.WithFooter("*Note: Data may be slightly inaccurate due to refresh times.");

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, here's the Old School RuneScape Ironman high scores for {playerName}:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneScape data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the stats. Please try again later.")
                );
            }
        }

        [SlashCommand("os_hciron_stats", "Search Old School RuneScape Hardcore Ironman hiscores for a player's stats")]
        public async Task GetOSHCIronStats(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                // Replace spaces with underscores in the player's name for the URL
                string urlPlayerName = playerName.Replace(" ", "_");
                string url = $"https://secure.runescape.com/m=hiscore_oldschool_hardcore_ironman/index_lite.ws?player={urlPlayerName}";
                string response = await _httpClient.GetStringAsync(url);
                string[] stats = response.Split('\n');

                var table = new StringBuilder();
                table.AppendLine("Skill           Rank        Level     XP   ");
                table.AppendLine("_______________________________________\n");

                for (int i = 0; i < stats.Length; i++)
                {
                    var stat = stats[i].Split(',');

                    if (stat.Length >= 3)
                    {
                        var rank = stat[0];
                        var skillName = GetSkillName(i).Replace("_", " "); // Replace underscores with spaces
                        var level = stat[1];
                        var experience = stat[2];

                        string formattedSkill = skillName.PadRight(14); // Adjust the padding as needed
                        string formattedRank = FormatRank(rank).PadRight(14); // Adjust the padding as needed
                        string formattedLevel = level.PadRight(8); // Adjust the padding as needed
                        string formattedXP = FormatXP(experience).PadRight(20); // Adjust the padding as needed

                        table.AppendLine($"{formattedSkill}{formattedRank}{formattedLevel}{formattedXP}");
                    }
                }

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{playerName}'s OS Hardcore Ironman Stats",
                    Color = new DiscordColor(252, 185, 0), // Gold color
                    Description = "```" + table.ToString() + "```"
                };

                string osHCIronLink = $"[View Full OS Hardcore Ironman Stats](https://secure.runescape.com/m=hiscore_oldschool_hardcore_ironman/a=12/compare?user1={urlPlayerName})";
                embed.AddField("Full Stats", osHCIronLink);
                embed.WithFooter("*Note: Data may be slightly inaccurate due to refresh times.");

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, here's the Old School RuneScape Hardcore Ironman high scores for {playerName}:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneScape data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the stats. Please try again later.")
                );
            }
        }

        [SlashCommand("os_uim_stats", "Search Old School RuneScape Ultimate Ironman hiscores for a player's stats")]
        public async Task GetOSUIMStats(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                // Replace spaces with underscores in the player's name for the URL
                string urlPlayerName = playerName.Replace(" ", "_");
                string url = $"https://secure.runescape.com/m=hiscore_oldschool_ultimate/index_lite.ws?player={urlPlayerName}";
                string response = await _httpClient.GetStringAsync(url);
                string[] stats = response.Split('\n');

                var table = new StringBuilder();
                table.AppendLine("Skill           Rank        Level     XP   ");
                table.AppendLine("_______________________________________\n");

                for (int i = 0; i < stats.Length; i++)
                {
                    var stat = stats[i].Split(',');

                    if (stat.Length >= 3)
                    {
                        var rank = stat[0];
                        var skillName = GetSkillName(i).Replace("_", " "); // Replace underscores with spaces
                        var level = stat[1];
                        var experience = stat[2];

                        string formattedSkill = skillName.PadRight(14); // Adjust the padding as needed
                        string formattedRank = FormatRank(rank).PadRight(14); // Adjust the padding as needed
                        string formattedLevel = level.PadRight(8); // Adjust the padding as needed
                        string formattedXP = FormatXP(experience).PadRight(20); // Adjust the padding as needed

                        table.AppendLine($"{formattedSkill}{formattedRank}{formattedLevel}{formattedXP}");
                    }
                }

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{playerName}'s OS Ultimate Ironman Stats",
                    Color = new DiscordColor(252, 185, 0), // Gold color
                    Description = "```" + table.ToString() + "```"
                };

                string osUIMLink = $"[View Full OS Ultimate Ironman Stats](https://secure.runescape.com/m=hiscore_oldschool_ultimate/a=12/compare?user1={urlPlayerName})";
                embed.AddField("Full Stats", osUIMLink);
                embed.WithFooter("*Note: Data may be slightly inaccurate due to refresh times.");

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, here's the Old School RuneScape Ultimate Ironman high scores for {playerName}:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneScape data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the stats. Please try again later.")
                );
            }
        }

        [SlashCommand("ge", "Check Grand Exchange data for an item by its ID")]
        public async Task GetGrandExchangeData(InteractionContext ctx, [Option("item", "The item ID")] long itemId)
        {
            try
            {
                // Get the item details using the provided item ID
                string detailUrl = $"https://services.runescape.com/m=itemdb_rs/api/catalogue/detail.json?item={itemId}";
                var detailResponse = await _httpClient.GetStringAsync(detailUrl);
                var itemDetail = JsonConvert.DeserializeObject<JObject>(detailResponse);

                var embed = new DiscordEmbedBuilder
                {
                    Title = itemDetail["item"]["name"].ToString(),
                    Color = new DiscordColor(252, 185, 0), // Gold color
                };

                embed.AddField("Description", itemDetail["item"]["description"].ToString());
                embed.AddField("Current Price", itemDetail["item"]["current"]["price"].ToString());
                embed.AddField("Price Trend", itemDetail["item"]["current"]["trend"].ToString());
                embed.AddField("Members Only", itemDetail["item"]["members"].ToString() == "true" ? "Yes" : "No");

                // You can add more fields as needed

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Here's the Grand Exchange data for the item with ID {itemId}:")
                    .AddEmbed(embed)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Grand Exchange data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the Grand Exchange data. Please try again later.")
                );
            }
        }


        [SlashCommand("rs_totalcount", "Get total player count for RuneScape")]
        public async Task GetTotalCount(InteractionContext ctx)
        {
            try
            {
                // Get the player count
                string playerCountUrl = "http://www.runescape.com/player_count.js?varname=iPlayerCount&callback=jQuery000000000000000_0000000000&_=0";
                string playerCountResponse = await _httpClient.GetStringAsync(playerCountUrl);

                // Get the total user count
                string totalUserCountUrl = "https://secure.runescape.com/m=account-creation-reports/rsusertotal.ws";
                string totalUserCountResponse = await _httpClient.GetStringAsync(totalUserCountUrl);

                // Parse the player count response
                var match = Regex.Match(playerCountResponse, @"iPlayerCount = (\d+);");

                if (match.Success)
                {
                    int playerCount = int.Parse(match.Groups[1].Value);
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = "RuneScape Total Player Count",
                        Color = new DiscordColor(252, 185, 0), // Gold color
                        Description = $"Currently, there are {playerCount} players online in RuneScape."
                    };

                    embed.AddField("Total User Count", totalUserCountResponse.Trim());

                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .AddEmbed(embed)
                        .AsEphemeral(true) // Make the response ephemeral
                    );
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Unable to retrieve player count information.")
                        .AsEphemeral(true) // Make the response ephemeral
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RuneScape data: {ex.Message}");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("An error occurred while fetching the information. Please try again later.")
                    .AsEphemeral(true) // Make the response ephemeral
                );
            }
        }


        private string FormatRank(string rank)
        {
            if (long.TryParse(rank, out long rankValue))
            {
                Console.WriteLine($"Parsed rank: {rankValue}");
                return rankValue.ToString("N0", CultureInfo.InvariantCulture);
            }

            Console.WriteLine($"Invalid rank format: {rank}");
            return rank;
        }
        // Define a function to map index to skill name based on the RuneScape hiscores order
        private string GetSkillName(int index)
        {
            string[] skillNames = {
                "Overall", "Attack", "Defence", "Strength", "Hitpoints", "Ranged", "Prayer", "Magic", "Cooking",
                "Woodcutting", "Fletching", "Fishing", "Firemaking", "Crafting", "Smithing", "Mining", "Herblore",
                "Agility", "Thieving", "Slayer", "Farming", "Runecrafting", "Hunter", "Construction", "Summoning", "Dungeoneering", "Divination",
                "Invention", "Archaeology", "Necromancy"
            };

            if (index >= 0 && index < skillNames.Length)
            {
                return skillNames[index];
            }

            return "Unknown";
        }

        // Format XP with commas for better readability
        private string FormatXP(string xp)
        {
            if (long.TryParse(xp, out long xpValue))
            {
                return xpValue.ToString("N0", CultureInfo.InvariantCulture);
            }

            return xp;
        }
    }
}
