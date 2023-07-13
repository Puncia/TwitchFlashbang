using System.Reflection;

[assembly: AssemblyVersion("0.1.*")]
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
            Application.Run(new App());
        }
    }
}