using Microsoft.Extensions.Configuration;

namespace TwitchFlashbang
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (!File.Exists("twitchflashbang.json"))
            {
                MessageBox.Show("Please create a twitchflashbang.json file first");
                return;
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("twitchflashbang.json", optional: false, reloadOnChange: true);

            AppConfig.Configuration = builder.Build();

            ApplicationConfiguration.Initialize();
            Application.Run(new Flashbang());
        }
    }
}