using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MySqlConnector;

namespace CryptoParserServer
{
    public class ServerSettings
    {
        public string Ip { get; set; }

        public int Port { get; set; }
    }

    public class DataBaseSettings
    {
        public string Server { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string DataBase { get; set; }
    }

    public class Settings
    {
        public ServerSettings Server { get; set; }

        public DataBaseSettings DataBaseSettings { get; set; }

        public string Version { get; set; }

        public bool DemoModeEnabled { get; set; }
    }

    public static class App
    {
        public static Settings Settings { get; set; }

        public static List<JsonClasses.Internal.Arbitrage> ArbitragesCache = new List<JsonClasses.Internal.Arbitrage>();

        public static string ArbitragesAns = "[]";

        public static MySqlConnection MySQLConnection { get; set; }

        public static void LoadSettings()
        {
            if (!File.Exists("Settings.json")) Settings = new Settings() {
                Server = new ServerSettings() {
                    Ip = "193.38.235.240",
                    Port = 9090
                },
                DataBaseSettings = new DataBaseSettings() {
                    DataBase = "cryptoparser",
                    Server = "localhost",
                    User = "root",
                    Password = "81WCXIh6G6#%"
                },
                Version = "1.0.0",
                DemoModeEnabled = false

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
