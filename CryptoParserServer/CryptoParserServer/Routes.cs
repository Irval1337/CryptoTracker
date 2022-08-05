using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WatsonWebserver;

namespace CryptoParserServer
{
    public static class Routes
    {
        public static string GetBestArbitragesString(string data)
        {
            try
            {
                if (String.IsNullOrEmpty(data))
                    return "{[]}";

                if (App.Settings.DemoModeEnabled) return App.ArbitragesAns;

                if (!data.Contains("?") || data.IndexOf("?") + 1 == data.Length) return "{[]}";
                data = data.Substring(data.IndexOf("?") + 1);

                string[] _params = data.Split('&');

                if (_params.Length != 1 || !_params[0].StartsWith("hwid=")) return "{[]}";

                var user = Database.Auth(_params[0].Split('=')[1]);
                if (user == null || user.Status == User.UserStatus.Registered) return "{[]}";

                if (user.LicenseExpiration < DateTime.Now && user.Status != User.UserStatus.Admin)
                {
                    Database.Deletelicense(_params[0].Split('=')[1]);
                    return "{[]}";
                }

                return App.ArbitragesAns;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task GetBestArbitrages(HttpContext ctx)
        {
            try
            {
                await ctx.Response.Send(GetBestArbitragesString(ctx.Request.Url.RawWithQuery));
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                await ctx.Response.Send(ex.Message);
            }
        }

        public static string AuthString(string data)
        {
            try
            {
                if (String.IsNullOrEmpty(data))
                    return "{}";
                if (App.Settings.DemoModeEnabled) return JsonConvert.SerializeObject(new User() 
                { Id = 1, LicenseExpiration = DateTime.Now.AddDays(10000), Status = User.UserStatus.Licensed, Username = "Demo" });

                if (!data.Contains("?") || data.IndexOf("?") + 1 == data.Length) return "{}";
                data = data.Substring(data.IndexOf("?") + 1);

                string[] _params = data.Split('&');

                if (_params.Length != 1 || !_params[0].StartsWith("hwid=")) return "{}";

                var user = Database.Auth(_params[0].Split('=')[1]);
                if (user == null) return "{}";

                if (user.LicenseExpiration < DateTime.Now && user.Status != User.UserStatus.Admin)
                {
                    Database.Deletelicense(_params[0].Split('=')[1]);
                    user.Status = User.UserStatus.Registered;
                }

                return JsonConvert.SerializeObject(user);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task Auth(HttpContext ctx)
        {
            try
            {
                await ctx.Response.Send(AuthString(ctx.Request.Url.RawWithQuery));
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                await ctx.Response.Send(ex.Message);
            }
        }

        public static string RegisterString(string data)
        {
            try
            {
                if (String.IsNullOrEmpty(data))
                    return "{}";

                if (!data.Contains("?") || data.IndexOf("?") + 1 == data.Length) return "{}";
                data = data.Substring(data.IndexOf("?") + 1);

                string[] _params = data.Split('&');

                if (_params.Length != 3 || !_params[0].StartsWith("hwid=") || !_params[1].StartsWith("user_hwid=") || !_params[2].StartsWith("username=")) return "{}";

                var admin = Database.Auth(_params[0].Split('=')[1]);
                if (admin == null || admin.Status != User.UserStatus.Admin) return "{}";

                var user = Database.Register(_params[1].Split('=')[1], _params[2].Split('=')[1]);
                if (user == null) return "{}";

                return JsonConvert.SerializeObject(user);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task Register(HttpContext ctx)
        {
            try
            {
                await ctx.Response.Send(RegisterString(ctx.Request.Url.RawWithQuery));
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                await ctx.Response.Send(ex.Message);
            }
        }

        public static string GiveLicenseString(string data)
        {
            try
            {
                if (String.IsNullOrEmpty(data))
                    return "ERROR";

                if (!data.Contains("?") || data.IndexOf("?") + 1 == data.Length) return "ERROR";
                data = data.Substring(data.IndexOf("?") + 1);

                string[] _params = data.Split('&');

                if (_params.Length != 3 || !_params[0].StartsWith("hwid=") || !_params[1].StartsWith("user_hwid=") || !_params[2].StartsWith("days_count=")) return "ERROR";

                var user = Database.Auth(_params[0].Split('=')[1]);
                if (user == null || user.Status != User.UserStatus.Admin) return "ERROR";

                return Database.Givelicense(_params[1].Split('=')[1], DateTime.Now.AddDays(Convert.ToInt32(_params[2].Split('=')[1]))) ? "OK" : "ERROR";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task GiveLicense(HttpContext ctx)
        {
            try
            {
                await ctx.Response.Send(GiveLicenseString(ctx.Request.Url.RawWithQuery));
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                await ctx.Response.Send(ex.Message);
            }
        }

        public static string DeleteLicenseString(string data)
        {
            try
            {
                if (String.IsNullOrEmpty(data))
                    return "ERROR";

                if (!data.Contains("?") || data.IndexOf("?") + 1 == data.Length) return "ERROR";
                data = data.Substring(data.IndexOf("?") + 1);

                string[] _params = data.Split('&');

                if (_params.Length != 2 || !_params[0].StartsWith("hwid=") || !_params[1].StartsWith("user_hwid=")) return "ERROR";

                var user = Database.Auth(_params[0].Split('=')[1]);
                if (user == null || user.Status != User.UserStatus.Admin) return "ERROR";

                return Database.Deletelicense(_params[1].Split('=')[1]) ? "OK" : "ERROR";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task DeleteLicense(HttpContext ctx)
        {
            try
            {
                await ctx.Response.Send(DeleteLicenseString(ctx.Request.Url.RawWithQuery));
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                await ctx.Response.Send(ex.Message);
            }
        }

        public static async Task CurrentVersion(HttpContext ctx)
        {
            try
            {
                await ctx.Response.Send(App.Settings.Version);
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                await ctx.Response.Send(ex.Message);
            }
        }

        public static string SetVersionString(string data)
        {
            try
            {
                if (String.IsNullOrEmpty(data))
                    return "ERROR";

                if (!data.Contains("?") || data.IndexOf("?") + 1 == data.Length) return "ERROR";
                data = data.Substring(data.IndexOf("?") + 1);

                string[] _params = data.Split('&');

                if (_params.Length != 2 || !_params[0].StartsWith("hwid=") || !_params[1].StartsWith("version=")) return "ERROR";

                var user = Database.Auth(_params[0].Split('=')[1]);
                if (user == null) return "ERROR";

                if (user.Status != User.UserStatus.Admin)
                    return "ERROR";

                App.Settings.Version = _params[1].Split('=')[1];
                App.SaveSettings();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task SetCurrentVersion(HttpContext ctx)
        {
            try
            {
                await ctx.Response.Send(SetVersionString(ctx.Request.Url.RawWithQuery));
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                await ctx.Response.Send(ex.Message);
            }
        }

        public static string SetDemoModeString(string data)
        {
            try
            {
                if (String.IsNullOrEmpty(data))
                    return "ERROR";

                if (!data.Contains("?") || data.IndexOf("?") + 1 == data.Length) return "ERROR";
                data = data.Substring(data.IndexOf("?") + 1);

                string[] _params = data.Split('&');

                if (_params.Length != 2 || !_params[0].StartsWith("hwid=") || !_params[1].StartsWith("mode=")) return "ERROR";

                var user = Database.Auth(_params[0].Split('=')[1]);
                if (user == null) return "ERROR";

                if (user.Status != User.UserStatus.Admin)
                    return "ERROR";

                App.Settings.DemoModeEnabled = _params[1].Split('=')[1].ToLower() == "true";
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task SetDemoMode(HttpContext ctx)
        {
            try
            {
                await ctx.Response.Send(SetDemoModeString(ctx.Request.Url.RawWithQuery));
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                await ctx.Response.Send(ex.Message);
            }
        }
    }
}
