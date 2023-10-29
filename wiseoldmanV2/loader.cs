namespace wiseoldmanV2
{
    internal static class loader
    {
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string version = "0.3.0";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("version: " + version);
            Console.WriteLine("W E L C O M E   T O   T H E   O L D    M A N   D I S C O R D   B O T !");
            Console.WriteLine("Created by Nath - 2023 - All rights reserved.");
            ApplicationConfiguration.Initialize();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[Loader] Starting Admin Panel...");
            Application.Run(new frmAdminPanel());
        }
    }
}