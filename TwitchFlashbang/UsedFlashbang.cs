using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchFlashbang
{
    public class UsedFlashbang
    {
        private string uid;
        private int nth;
        private string foregroundWindow;
        private double initialOpacity;
        private double finalOpacity;
        private bool forceAbort;
        private bool enableAfterImage;
        private double time;

        public string UID { get => uid; set => uid = value; }
        public int Nth { get => nth; set => nth = value; }
        public string ForegroundWindow { get => foregroundWindow; set => foregroundWindow = value; }
        public double InitialOpacity { get => initialOpacity; set => initialOpacity = value; }
        public double FinalOpacity { get => finalOpacity; set => finalOpacity = value; }
        public bool ForceAbort { get => forceAbort; set => forceAbort = value; }
        public bool EnableAfterImage { get => enableAfterImage; set => enableAfterImage = value; }
        public double Time { get => time; set => time = value; }
    }
}
