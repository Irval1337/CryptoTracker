using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WatsonWebserver;
using NLog.Config;
using NLog.Targets;
using NLog;
using NLog.Conditions;
using MySqlConnector;
using System.IO.Compression;

namespace CryptoParserServer
{
    internal class Program
    {
        public static string Layout;

        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static bool needClose = false;

        static async Task Main(string[] args)
        {
            await InitializeLogger();
            App.LoadSettings();

            if (!Directory.Exists("client"))
            {
                Logger.Error("The client's file directory is missing");
                return;
            }

            if (File.Exists("client.zip")) File.Delete("client.zip");
            ZipFile.CreateFromDirectory("client", "client.zip");

            Server s = new Server(App.Settings.Server.Ip, App.Settings.Server.Port, false, DefaultRoute);

            s.Routes.Static.Add(HttpMethod.GET, "/GetBestArbitrages", Routes.GetBestArbitrages);
            s.Routes.Static.Add(HttpMethod.GET, "/Auth", Routes.Auth);
            s.Routes.Static.Add(HttpMethod.GET, "/Register", Routes.Register);
            s.Routes.Static.Add(HttpMethod.GET, "/GiveLicense", Routes.GiveLicense); 
            s.Routes.Static.Add(HttpMethod.GET, "/DeleteLicense", Routes.DeleteLicense);
            s.Routes.Static.Add(HttpMethod.GET, "/CurrentVersion", Routes.CurrentVersion);
            s.Routes.Static.Add(HttpMethod.GET, "/CurrentVersionInfo", Routes.CurrentVersionInfo);
            s.Routes.Static.Add(HttpMethod.GET, "/GetFiles", Routes.GetFiles);
            s.Routes.Static.Add(HttpMethod.GET, "/SetCurrentVersion", Routes.SetCurrentVersion);
            s.Routes.Static.Add(HttpMethod.GET, "/SetDemoMode", Routes.SetDemoMode);
            s.Start();

            Logger.Info("Server started!");

            string connStr = $"server={App.Settings.DataBaseSettings.Server};user={App.Settings.DataBaseSettings.User};database={App.Settings.DataBaseSettings.DataBase};password={App.Settings.DataBaseSettings.Password};";
            App.MySQLConnection = new MySqlConnection(connStr);
            App.MySQLConnection.Open();

            Logger.Info("MySQL connection opened!");

            СryptorankApi сryptorankApi = new СryptorankApi();

            await Task.Factory.StartNew(() =>
            {
                while (!needClose)
                {
                    Task.Factory.StartNew(() =>
                    {
                        App.ArbitragesCache = сryptorankApi.GetBestArbitrages();
                        App.ArbitragesAns = JsonConvert.SerializeObject(App.ArbitragesCache);
                    }).Wait();
                }
            });

            Console.ReadKey();
            needClose = true;

            App.SaveSettings();
            Environment.Exit(0);
        }

        public static async Task InitializeLogger()
        {
            #region NLog Initializator

            // NLOG Code start
            var config = new NLog.Config.LoggingConfiguration();
            LogManager.Configuration = new LoggingConfiguration();
            // Targets where to log to: File and Console
            Layout = @"[${date:format=yyyy-MM-dd HH\:mm\:ss}] [${logger}/${uppercase: ${level}}] [THREAD: ${threadid}] >> ${message} ${exception: format=ToString}";
            var consoleTarget = new ColoredConsoleTarget("Console Target")
            {
                Layout = @"${counter}|[${date:format=yyyy-MM-dd HH\:mm\:ss}] [${logger}/${uppercase: ${level}}] [THREAD: ${threadid}] >> ${message} ${exception: format=ToString}"
            };
            var logfile = new FileTarget();

            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            logfile.CreateDirs = true;
            logfile.ArchiveEvery = FileArchivePeriod.Day;
            logfile.ArchiveAboveSize = 64 * 1024 * 1024; // 64 MB
            logfile.FileName = $"logs{Path.DirectorySeparatorChar}latest.log";
            logfile.ArchiveFileName = $"logs{Path.DirectorySeparatorChar}{{#}}.zip";
            logfile.ArchiveNumbering = ArchiveNumberingMode.DateAndSequence;
            logfile.ArchiveOldFileOnStartup = true;
            logfile.ArchiveDateFormat = "MM.dd.yyyy";
            logfile.AutoFlush = true;
            logfile.EnableArchiveFileCompression = true;
            logfile.LineEnding = LineEndingMode.CRLF;
            logfile.Layout = Layout;
            logfile.FileNameKind = FilePathKind.Absolute;
            logfile.ArchiveFileKind = FilePathKind.Relative;
            logfile.ConcurrentWrites = false;
            logfile.KeepFileOpen = true;
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
            // Apply config
            NLog.LogManager.Configuration = config;
            #region NLog Colors

            var Trace = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Trace"),
                ForegroundColor = ConsoleOutputColor.Yellow
            };
            consoleTarget.RowHighlightingRules.Add(Trace);
            ConsoleRowHighlightingRule Debug = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Debug"),
                ForegroundColor = ConsoleOutputColor.DarkCyan
            };
            consoleTarget.RowHighlightingRules.Add(Debug);
            var Info = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Info"),
                ForegroundColor = ConsoleOutputColor.Green
            };
            consoleTarget.RowHighlightingRules.Add(Info);
            var Warn = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Warn"),
                ForegroundColor = ConsoleOutputColor.DarkYellow
            };
            consoleTarget.RowHighlightingRules.Add(Warn);
            var Error = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Error"),
                ForegroundColor = ConsoleOutputColor.DarkRed
            };
            consoleTarget.RowHighlightingRules.Add(Error);
            var Fatal = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Fatal"),
                ForegroundColor = ConsoleOutputColor.Black,
                BackgroundColor = ConsoleOutputColor.DarkRed
            };
            consoleTarget.RowHighlightingRules.Add(Fatal);

            #endregion NLog Colors
            #endregion NLog Initializator
        }

        #region DefaultRoutes
        static async Task DefaultRoute(HttpContext ctx)
        {
            await ctx.Response.Send("Default route.");
        }
        #endregion
    }
}
