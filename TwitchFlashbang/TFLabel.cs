using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchFlashbang
{
    internal class TFLabel : Label
    {
        public TFLabel()
        {
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Region = Region.FromHrgn(WinAPI.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
        }
    }
}
