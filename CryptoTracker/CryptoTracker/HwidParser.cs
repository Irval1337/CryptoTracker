using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CryptoTracker
{
    internal static class HWID
    {
        private static string hardwareId;

        internal static string GetHardwareId() => hardwareId;

        private static void SetHardwareId(string value) => hardwareId = value;

        internal static string ComputeHash(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);

            using (var sha = new SHA256Managed())
            {
                data = sha.ComputeHash(data, 0, data.Length);
            }

            var hash = new StringBuilder();

            foreach (var _byte in data)
            {
                hash.Append(_byte.ToString("X2"));
            }

            return hash.ToString().ToUpper();
        }

        static HWID() => SetHardwareId(ComputeHash(GetCpuName() + GetMainboardIdentifier() + GetBiosIdentifier()));

        internal static string GetBiosIdentifier()
        {
            try
            {
                var biosIdentifier = string.Empty;
                var query = "SELECT * FROM Win32_BIOS";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        biosIdentifier = mObject["Manufacturer"].ToString();
                        break;
                    }
                }

                return (!string.IsNullOrEmpty(biosIdentifier)) ? biosIdentifier : "N/A";
            }
            catch
            {
            }

            return "Unknown";
        }

        internal static string GetMainboardIdentifier()
        {
            try
            {
                var mainboardIdentifier = string.Empty;
                var query = "SELECT * FROM Win32_BaseBoard";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        mainboardIdentifier = mObject["Manufacturer"].ToString() + mObject["SerialNumber"].ToString();
                        break;
                    }
                }

                return (!string.IsNullOrEmpty(mainboardIdentifier)) ? mainboardIdentifier : "N/A";
            }
            catch
            {
            }

            return "Unknown";
        }

        internal static string GetCpuName()
        {
            try
            {
                var cpuName = string.Empty;
                var query = "SELECT * FROM Win32_Processor";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        cpuName += mObject["Name"].ToString() + "; ";
                    }
                }
                cpuName = RemoveEnd(cpuName);

                return (!string.IsNullOrEmpty(cpuName)) ? cpuName : "N/A";
            }
            catch
            {
            }

            return "Unknown";
        }

        internal static int GetTotalRamAmount()
        {
            try
            {
                var installedRAM = 0;
                var query = "Select * From Win32_ComputerSystem";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        var bytes = (Convert.ToDouble(mObject["TotalPhysicalMemory"]));
                        installedRAM = (int)(bytes / 1048576);
                        break;
                    }
                }

                return installedRAM;
            }
            catch
            {
                return -1;
            }
        }

        internal static string GetGpuName()
        {
            try
            {
                var gpuName = string.Empty;
                var query = "SELECT * FROM Win32_DisplayConfiguration";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        gpuName += mObject["Description"].ToString() + "; ";
                    }
                }
                gpuName = RemoveEnd(gpuName);

                return (!string.IsNullOrEmpty(gpuName)) ? gpuName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }

        internal static string GetLanIp()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var gatewayAddress = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (gatewayAddress != null) //exclude virtual physical nic with no default gateway
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        ni.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (var ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily != AddressFamily.InterNetwork ||
                                ip.AddressPreferredLifetime == uint.MaxValue) // exclude virtual network addresses
                            {
                                continue;
                            }

                            return ip.Address.ToString();
                        }
                    }
                }
            }

            return "-";
        }

        internal static string GetMacAddress()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    ni.OperationalStatus == OperationalStatus.Up)
                {
                    var foundCorrect = false;
                    foreach (var ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily != AddressFamily.InterNetwork ||
                            ip.AddressPreferredLifetime == uint.MaxValue) // exclude virtual network addresses
                        {
                            continue;
                        }

                        foundCorrect = (ip.Address.ToString() == GetLanIp());
                    }

                    if (foundCorrect)
                    {
                        return FormatMacAddress(ni.GetPhysicalAddress().ToString());
                    }
                }
            }

            return "-";
        }

        internal static string FormatMacAddress(string macAddress) => (macAddress.Length != 12)
                ? "00:00:00:00:00:00"
                : Regex.Replace(macAddress, "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", "$1:$2:$3:$4:$5:$6");

        internal static string DriveTypeName(DriveType type)
        {
            switch (type)
            {
                case DriveType.Fixed:
                    return "Local Disk";
                case DriveType.Network:
                    return "Network Drive";
                case DriveType.Removable:
                    return "Removable Drive";
                default:
                    return type.ToString();
            }
        }

        internal static string FormatScreenResolution(Rectangle resolution) => $"{resolution.Width}x{resolution.Height}";

        internal static string RemoveEnd(string input)
        {
            if (input.Length > 2)
            {
                input = input.Remove(input.Length - 2);
            }

            return input;
        }
    }
}