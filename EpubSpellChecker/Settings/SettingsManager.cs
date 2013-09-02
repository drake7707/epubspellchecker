using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EpubSpellChecker
{
    class SettingsManager
    {

        private static Settings currentSettings;

        internal static Settings GetSettings()
        {
            if (currentSettings == null)
            {
                string path = GetSettingsPath();
                string xml;
                if (File.Exists(path))
                {
                    xml = File.ReadAllText(path);
                    currentSettings = (Settings)SimpleXmlSerializer.Deserialize(xml);
                }
                else
                    currentSettings = Settings.GetDefault();
            }
            return currentSettings;
        }

        internal static void SaveSettings(Settings settings)
        {
            string path = GetSettingsPath();
            string xml = SimpleXmlSerializer.Serialize(settings);
            File.WriteAllText(path, xml);
        }


        private static string GetSettingsPath()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string settingsPath = System.IO.Path.Combine(path, "settings.xml");
            return settingsPath;
        }


    }
}
