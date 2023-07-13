using System.Diagnostics;

namespace TwitchFlashbang
{
    public class ViewManager : TablessControl
    {
        public event Action<string> WarningFired;
        private View _selectedTab;

        public ViewManager()
        {
            _selectedTab = 0;
        }

        public enum View
        {
            TwitchAuthentication,
            Rewards,
            AppStatus
        }

        public enum MissingCredentials
        {
            None = 0,
            MissingChannelName = 1,
            MissingClientID = 2,
            MissingClientSecret = 4
        }

        public void Switch(View view, MissingCredentials warning = MissingCredentials.None)
        {
            if (view == 0)
            {
                foreach (Control c in TabPages[0].Controls)
                {
                    if (!c.Enabled)
                    {
                        Invoke(() => c.Enabled = true);
                    }
                }

                string wlist = string.Empty;
                if ((warning & MissingCredentials.MissingChannelName) != 0)
                {
                    if (wlist != string.Empty)
                    {
                        wlist += ", ";
                    }

                    wlist = "Channel Name";
                }
                if ((warning & MissingCredentials.MissingClientID) != 0)
                {
                    if (wlist != string.Empty)
                    {
                        wlist += ", ";
                    }

                    wlist += "Client ID";
                }
                if ((warning & MissingCredentials.MissingClientSecret) != 0)
                {
                    if (wlist != string.Empty)
                    {
                        wlist += ", ";
                    }

                    wlist += "Client Secret";
                }

                Debug.WriteLine($"ViewManager warning: {wlist}");

                if (wlist != string.Empty)
                {
                    WarningFired?.Invoke($"The following fields are incorrect: {wlist}");
                }
                else
                {
                    WarningFired?.Invoke(string.Empty);
                }
            }

            _selectedTab = view;
            BeginInvoke(() => SelectTab((int)_selectedTab));
        }
    }
}
