using System;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Globalization;
using DSharpPlus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
            // Create an instance of RustMemeManager and call the method to get a random Rust meme
            ulong targetGuildId = 715197288984870932; //w00ters rank hub
            ulong targetChannelId = 718127567273721897; //Channel to scan for Rust Memes
            var memeManager = new RustMemeManager(ctx.Client, targetGuildId, targetChannelId);
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


        [SlashCommand("stats", "Search RuneScape hiscores for a player's stats (RS3)")]
        public async Task GetStats(InteractionContext ctx, [Option("player", "The player's name")] string playerName)
        {
            try
            {
                string url = $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={playerName}";
                string response = await _httpClient.GetStringAsync(url);
                string[] stats = response.Split('\n');

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{playerName}'s RS3 Stats",
                    Color = new DiscordColor(252, 185, 0), // Green color
                };

                string summary = "";

                for (int i = 0; i < stats.Length; i++)
                {
                    var stat = stats[i].Split(',');

                    if (stat.Length >= 3)
                    {
                        var skillName = GetSkillName(i);
                        var level = stat[1];
                        var experience = stat[2];

                        string formattedXP = FormatXP(experience);

                        summary += $"**{skillName}:** Level {level}, Exp {formattedXP}\n";
                    }
                }

                embed.Description = summary;
                string rsLink = $"[View Full Stats](https://secure.runescape.com/m=hiscore/a=12/compare?user1={playerName})";
                embed.AddField("Full Stats", rsLink);
                embed.WithFooter("*Note: Data may be slightly inaccurate due to Refresh times.");

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{ctx.User.Mention}, heres the highscores stats for {playerName}:")
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
