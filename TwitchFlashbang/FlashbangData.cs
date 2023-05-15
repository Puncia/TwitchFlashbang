using System.Diagnostics;

namespace TwitchFlashbang
{
    public class FlashbangData : IDisposable
    {
        public FlashbangData(string ID, string Redeemer)
        {
            fadingStopwatch = new Stopwatch();
            flashingStopwatch = new Stopwatch();
            this.ID = ID;
            this.Redeemer = Redeemer;
        }

        public Stopwatch flashingStopwatch;
        public Stopwatch fadingStopwatch;
        public string ID;
        public string Redeemer;

        public void Dispose()
        {
            fadingStopwatch.Stop();
            flashingStopwatch.Stop();
        }
    }
}
