using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker
{
    public class Settings
    {
        public List<string> BlackListed { get; set; }

        public int MinPercent { get; set; }

        public bool DrawCurrencies { get; set; }
    }

    public static class App
    {
        public static Settings Settings { get; set; }

        public static string Server = "http://193.38.235.240:9090/";

        public static string Version = "1.0.0";

        public static void LoadSettings()
        {
            if (!File.Exists("Settings.json")) Settings = new Settings()
            {
                BlackListed = new List<string>(),
                MinPercent = 0,
                DrawCurrencies = true
            };
            else Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("Settings.json"));
            SaveSettings();
        }

        public static void SaveSettings()
        {
            File.WriteAllText("Settings.json", JsonConvert.SerializeObject(Settings));
        }
    }
}
