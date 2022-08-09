using RestSharp;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

[DllImport("User32.dll", CharSet = CharSet.Unicode)]
static extern int MessageBox(IntPtr h, string m, string c, int type);

RestClient client = new RestClient("http://193.38.235.240:9090/");
RestRequest request = new RestRequest("GetFiles", Method.Get);
RestResponse response = client.Execute(request);

File.WriteAllBytes("client.zip", response.RawBytes);

ZipFile.ExtractToDirectory("client.zip", Directory.GetCurrentDirectory(), true);

File.Delete("client.zip");

Encoding encoding = Encoding.GetEncoding("utf-8");
request = new RestRequest("CurrentVersionInfo", Method.Get);
response = client.Execute(request);

MessageBox((IntPtr)0, "Информация о последнем обновлении:\n" + encoding.GetString(response.RawBytes), "Программа успешно обновлена", 0);

Process.Start("CryptoTracker.exe");