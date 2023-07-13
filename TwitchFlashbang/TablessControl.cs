namespace TwitchFlashbang
{
    public partial class TablessControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (m.Msg == 0x1328 && !DesignMode)
            {
                m.Result = 1;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
