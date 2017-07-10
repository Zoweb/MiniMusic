using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;
using Ookii.Dialogs;

namespace MiniMusic.Installer.Version2.Extractor
{
    static class Program
    {
        private static string InstallationDirectory;

        private static string GetTempFileName()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        private static LoggerOutputEventArgs ParseCommand(string input)
        {
            return new LoggerOutputEventArgs
            {
                Message = input
            };
        }

        private static void SendLine(LoggerOutputEventArgs command)
        {
            Console.WriteLine("response from " + command.Command + ";" + command.Id + ";" + command.Value);
        }

        static void Main()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line == null) continue;

                var command = ParseCommand(line);

                switch (command.Command)
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "create folder":
                        Directory.CreateDirectory(command.Message);
                        break;
                    case "download":
                        var webClient = new WebClient();
                        var filePath = GetTempFileName();
                        webClient.DownloadFile(new Uri(command.Message), filePath);
                        Console.WriteLine(filePath);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
