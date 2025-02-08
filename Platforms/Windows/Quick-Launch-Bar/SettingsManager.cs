using System;

namespace Quick_Launch_Bar
{
    class SettingsManager
    {
        private readonly Windows.Storage.ApplicationDataContainer localSettings =
            Windows.Storage.ApplicationData.Current.LocalSettings;

        //private static readonly string SettingsFolder ="JSON";

        public bool SaveBoolSetting(string name, bool value)
        {
            localSettings.Values[name] = value;

            return true;
        }

        public bool CheckBoolSetting(string name)
        {
            Object value = localSettings.Values[name];

            if (value == null)
                return false;

            bool result = (bool)value;

            return result;
        }
    }
}
