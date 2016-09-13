using System;
using System.Linq;

namespace Navyzbra.Printer.Library.Common
{
    public static class NZHelper
    {
        public static string[] ConvertPrintFormat(string text)
        {
            string[] pattern = { @"\n" };
            return text.ToString()
                .Replace(System.Environment.NewLine, @"\n")
                .Replace(@"[", string.Empty)
                .Replace(@"]", string.Empty)
                .Replace(@"{{t}}", "    ").Split(pattern, StringSplitOptions.None);
        }

        public static string HashPassword(string salt, string password) {
            string input =  salt + password;
            var provider = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            var encoding = new System.Text.UTF8Encoding();
            var bytes = provider.ComputeHash(encoding.GetBytes(input));
            return Bin2Hex(bytes).ToString().ToLower();
        }
        
        private static string Bin2Hex(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public static string GetLocalIP()
        {
            string localIP = null;
            using (System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(
                System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, 0))
            {
                socket.Connect("10.0.2.4", 65530);
                System.Net.IPEndPoint endPoint = socket.LocalEndPoint as System.Net.IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }



    }
}