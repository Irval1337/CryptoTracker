using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CryptoTracker
{
    public class Settings
    {
        public List<string> BlackListedMarkets = new List<string>();

        public List<string> BlackListedCoins = new List<string>();

        public int MinPercent = 0;

        public int MaxPercent = 0;

        public bool DrawCurrencies = true;

        public bool DrawFiats = true;
    }

    public static class App
    {
        public static Settings Settings { get; set; }

        public static string Server = "http://193.38.235.240:9090/";

        public static string Version = "1.0.1";

        public static void LoadSettings()
        {
            if (!File.Exists("Settings.json"))
            {
                Settings = new Settings();
                SaveSettings();
            }
            else Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("Settings.json"));
        }

        public static void SaveSettings()
        {
            File.WriteAllText("Settings.json", JsonConvert.SerializeObject(Settings));
        }
    }
}
