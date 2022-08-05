using CryptoTrackerAdmin;
using RestSharp;
using System.Text;

string hwid = HWID.GetHardwareId();

RestClient client = new RestClient("http://193.38.235.240:9090/"); //

RestRequest request = new RestRequest("CurrentVersion", Method.Get);
RestResponse response = client.Execute(request);
Encoding encoding = Encoding.GetEncoding("utf-8");

Console.WriteLine("Текущая версия: " + encoding.GetString(response.RawBytes));
Console.WriteLine("HWID: " + hwid + "\n");

Console.WriteLine("Доступные команды:\n/Auth <HWID пользователя> - получить информацию о пользователе\n/Register <HWID пользователя> <Имя пользователя> - регистрация нового пользователя\n/GiveLicense <HWID пользователя> <Количество дней> - Выдать лицензию пользователю по HWID`у\n/DelLicense <HWID пользователя> - забрать лицензию у пользователя\n/SetVersion <Версия в формате xx.xx.xx> - изменить текущую версию программы\n/SetDemoMode <true/false> - Включить/выключить режим бесплатной демоверсии");

while (true)
{
    Console.Write("\n> ");

    string command = Console.ReadLine();
    var data = command.Split(' ');
    switch(data[0])
    {
        case "/Register":
            request = new RestRequest("Register", Method.Get);

            request.AddParameter("hwid", hwid);
            request.AddParameter("user_hwid", data[1]);
            request.AddParameter("username", data[2]);

            response = client.Execute(request);
            Console.WriteLine(encoding.GetString(response.RawBytes));
            break;
        case "/GiveLicense":
            request = new RestRequest("GiveLicense", Method.Get);

            request.AddParameter("hwid", hwid);
            request.AddParameter("user_hwid", data[1]);
            request.AddParameter("days_count", data[2]);

            response = client.Execute(request);
            Console.WriteLine(encoding.GetString(response.RawBytes));
            break;
        case "/DelLicense":
            request = new RestRequest("DeleteLicense", Method.Get);

            request.AddParameter("hwid", hwid);
            request.AddParameter("user_hwid", data[1]);

            response = client.Execute(request);
            Console.WriteLine(encoding.GetString(response.RawBytes));
            break;
        case "/Auth":
            request = new RestRequest("Auth", Method.Get);

            request.AddParameter("hwid", data[1]);

            response = client.Execute(request);
            Console.WriteLine(encoding.GetString(response.RawBytes));
            break;
        case "/SetVersion":
            request = new RestRequest("SetCurrentVersion", Method.Get);

            request.AddParameter("hwid", hwid);
            request.AddParameter("version", data[1]);

            response = client.Execute(request);
            Console.WriteLine(encoding.GetString(response.RawBytes));
            break;
        case "/SetDemoMode":
            request = new RestRequest("SetDemoMode", Method.Get);

            request.AddParameter("hwid", hwid);
            request.AddParameter("mode", data[1].ToLower());

            response = client.Execute(request);
            Console.WriteLine(encoding.GetString(response.RawBytes));
            break;
        default: 
            Console.WriteLine("Неизвестная команда");
            break;
    }
}