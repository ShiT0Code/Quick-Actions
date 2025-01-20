using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;

namespace Quick_Launch_Bar
{
    class SettingsManager
    {
        Windows.Storage.ApplicationDataContainer localSettings;
        Windows.Storage.StorageFolder localFolder;

        public SettingsManager()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        }

        public bool IsWelcome()
        {
            Object value = localSettings.Values["IsWelcomed"];
            if (value == null)
                value = "false";

            if (value.ToString() != "true")
                return false;

            return true;
        }

        public bool Welcomed()
        {
            localSettings.Values["IsWelcomed"] = "true";

            Object value = localSettings.Values["IsWelcomed"];

            return true;
        }
    }
}
