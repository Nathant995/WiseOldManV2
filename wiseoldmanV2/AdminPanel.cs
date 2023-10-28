using DSharpPlus.Entities;


namespace wiseoldmanV2
{
    public partial class frmAdminPanel : Form
    {
        Bot _bot = new Bot();
        private bool isGameServerRunning = false; // Initially, the game server is not running


        public frmAdminPanel()
        {

            InitializeComponent();
            Console.WriteLine("[GUI] Panel Loaded.");
            StartPanelTimers();
        }

        private void StartPanelTimers()
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            tmrPanelTime.Start(); // Start the timer
            Console.WriteLine("[AdminPanel] Starting Global Timers...");
            // Subscribe to the Tick event
            tmrPanelTime.Tick += tmrPanelTime_Tick;
            tmrPanelTime.Interval = 1000; // Set the interval to 1 second
            Console.ForegroundColor = ConsoleColor.Green;
            tmrPanelTime.Start(); // Start the timer
            Console.WriteLine("[AdminPanel]  Global Timers Started!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void StopPanelTimers()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            tmrPanelTime.Stop();
            Console.WriteLine("[AdminPanel] Global Timer Stopped.");
            Console.ForegroundColor = ConsoleColor.White;

        }

        private void btnStartBot_Click(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[BOT] Bot is starting!");
            Console.ForegroundColor = ConsoleColor.White;
            _bot.StartAsync();
            btnStartBot.Text = "Running!";
            btnStartBot.ForeColor = Color.Green;
            btnStatusSet.Enabled = true;


        }


        private void btnStopBot_Click_1(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[BOT] Attempting to stop Server...");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            _bot.StopAsync();
            btnStartBot.Text = "Start Server";
            btnStartBot.ForeColor = Color.Black;
            btnStatusSet.Enabled = false;


        }

        private async void btnStatusSet_Click(object sender, EventArgs e)
        {
            string statusText = txtStatusInput.Text;

            try
            {
                if (_bot != null) // Check if the bot instance is not null and is connected to Discord
                {
                    if (!string.IsNullOrWhiteSpace(statusText))
                    {
                        await _bot.SetStatusAsync(statusText, ActivityType.Watching);

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Status text cannot be empty.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Bot is not connected to Discord or the bot instance is null.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting bot status: {ex.Message}");
            }
        }

        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            string messageType = "";

            if (rdoAnnouncement.Checked)
            {
                messageType = "announcement";
            }
            else if (rdoTest.Checked)
            {
                messageType = "test";
            }
            else if (rdoUpdate.Checked)
            {
                messageType = "update";
            }

            string messageText = rtbMessageInput.Text;

            var adminMessageManager = new AdminMessageManager(_bot.Client);
            await adminMessageManager.SendAdminMessage(messageType, messageText);

            Console.WriteLine($"[MessageSender] Sent {messageType} message: {messageText}");
            MessageBox.Show($"Sent {messageType} message: {messageText}", "Message Sent");
        }

        private void frmAdminPanel_Load(object sender, EventArgs e)
        {

        }

        private void tmrPanelTime_Tick(object sender, EventArgs e)
        {
            // Get the current date and time
            DateTime currentTime = DateTime.Now;

            // Format the date and time as "Monday 20th October 2023 : 08:03 AM"
            string formattedDateTime = currentTime.ToString("dddd d\\s MMMM yyyy : hh:mm:ss tt");

            // Update the label text
            lblPanelTime.Text = formattedDateTime;
        }


    }
}



