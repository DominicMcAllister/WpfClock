using System.Configuration;

namespace Clock.ConfigSettings
{
    public static class ClockConfiguration
    {
        public enum Preferences
        {
            DateFormat,
            TimeFormat
        };

        public static string GetPreference(Preferences p)
        {
            return ConfigurationManager.AppSettings[p.ToString()];
        }

        public static void SetPreference(Preferences p, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var key = p.ToString();
            if (configuration.AppSettings.Settings[key] != null) {
                configuration.AppSettings.Settings[key].Value = value;
            }
            else {
                configuration.AppSettings.Settings.Add(key, value);
            }

            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}