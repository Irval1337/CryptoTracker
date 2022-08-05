using System;
using System.Security.Cryptography;
using System.Text;
using MySqlConnector;

namespace CryptoParserServer
{
    public static class Database
    {
        public static User Auth(string hwid)
        {
            try
            {
                if (App.MySQLConnection.State == System.Data.ConnectionState.Closed) App.MySQLConnection.Open();
                string sql = "SELECT * FROM users WHERE HWID = @0";

                using (MySqlCommand command = new MySqlCommand(sql, App.MySQLConnection))
                {
                    command.Parameters.AddWithValue("@0", hwid);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        User userInstanse = null;

                        while (reader.Read())
                        {
                            userInstanse = new User();
                            userInstanse.Id = reader.GetInt32(0);
                            userInstanse.Username = reader.GetString(1);
                            userInstanse.LicenseExpiration = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(reader.GetInt32(3));
                            userInstanse.Status = (User.UserStatus)reader.GetInt32(4);
                        }

                        reader.Close();

                        return userInstanse;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                return null;
            }
        }

        public static User Register(string hwid, string username)
        {
            try
            {
                if (App.MySQLConnection.State == System.Data.ConnectionState.Closed) App.MySQLConnection.Open();
                if (Auth(hwid) != null) return null;

                User userInstanse = new User() { 
                    Id = GetLastId() + 1,
                    Username = username,
                    Status = User.UserStatus.Registered,
                    LicenseExpiration = new DateTime(1970, 1, 1)
                };

                string sql = $"INSERT INTO users (Id, Username, HWID, LicenseExpiration, Status) "
                    + $"VALUES ({userInstanse.Id}, @0, @1, {0}, {(int)userInstanse.Status})";

                using (MySqlCommand command = new MySqlCommand(sql, App.MySQLConnection))
                {
                    command.Parameters.AddWithValue("@0", username);
                    command.Parameters.AddWithValue("@1", hwid);
                    command.ExecuteNonQuery();

                    return userInstanse;
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                return null;
            }
        }

        public static bool Givelicense(string hwid, DateTime period)
        {
            if (App.MySQLConnection.State == System.Data.ConnectionState.Closed) App.MySQLConnection.Open();
            var user = Auth(hwid);
            if (user == null) return false;

            string sql = $"UPDATE users SET LicenseExpiration=@0, Status=@1 WHERE HWID=@2";

            using (MySqlCommand command = new MySqlCommand(sql, App.MySQLConnection))
            {
                command.Parameters.AddWithValue("@0", ((DateTimeOffset)period).ToUnixTimeSeconds());
                command.Parameters.AddWithValue("@1", user.Status == User.UserStatus.Admin ? 2 : 1);
                command.Parameters.AddWithValue("@2", hwid);

                command.ExecuteNonQuery();

                return true;
            }
        }

        public static bool Deletelicense(string hwid)
        {
            if (App.MySQLConnection.State == System.Data.ConnectionState.Closed) App.MySQLConnection.Open();
            var user = Auth(hwid);
            if (user == null) return false;

            string sql = $"UPDATE users SET Status=@0, LicenseExpiration=0 WHERE HWID=@1";

            using (MySqlCommand command = new MySqlCommand(sql, App.MySQLConnection))
            {
                command.Parameters.AddWithValue("@0", user.Status == User.UserStatus.Admin ? 2 : 0);
                command.Parameters.AddWithValue("@1", hwid);

                command.ExecuteNonQuery();

                return true;
            }
        }

        public static int GetLastId()
        {
            try
            {
                if (App.MySQLConnection.State == System.Data.ConnectionState.Closed) App.MySQLConnection.Open();
                string sql = "SELECT COUNT(*) FROM users";
                MySqlCommand command = new MySqlCommand(sql, App.MySQLConnection);
                int data = Convert.ToInt32(command.ExecuteScalar());

                return data;
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                return 0;
            }
        }
    }

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime LicenseExpiration { get; set; }

        public UserStatus Status { get; set; }

        public enum UserStatus { Registered, Licensed, Admin };
    }
}
