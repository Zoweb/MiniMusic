using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using static MiniMusic.WinFormsInstaller.InstallationCommand.Commands;

namespace MiniMusic.WinFormsInstaller
{
    public class InstallationCommand
    {
        public enum Commands
        {
            Download,
            CreateFolder,
            RunProgram,
            Copy,
            Shortcut,
            DeleteDirectory,
            FileAssociation,
            Delegate
        }

        public Commands Command;
        public string CommandArg1;
        public string CommandArg2;
        public string CommandArg3;
        public RunDelegate CommandArg4;

        private ListBox.ObjectCollection logger;

        public delegate bool RunDelegate();

        public InstallationCommand(Commands command, string arg1 = null, string arg2 = null, string arg3 = null, RunDelegate arg4 = null)
        {
            Command = command;
            CommandArg1 = arg1;
            CommandArg2 = arg2;
            CommandArg3 = arg3;
            CommandArg4 = arg4;
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
            Form1.Instance.Invoke(new Action(() =>
            {
                logger.Add(message);
                var list = Form1.Logger;
                var visibleItems = list.ClientSize.Height / list.ItemHeight;
                list.TopIndex = Math.Max(list.Items.Count - visibleItems + 1, 0);
            }));
        }

        public static void StartUAC(string locationVal, bool doSkip = false)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    Arguments = $"Start_At_Next_Section \"{locationVal}\" {doSkip}",
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    WindowStyle = ProcessWindowStyle.Normal,
                    CreateNoWindow = false,
                    Verb = "runas"
                });
                Application.Exit();
            }
            catch (Win32Exception)
            {
                MessageBox.Show(
                    "The directory you selected requires administrator privilages. You will need to select another folder after restarting the installer.",
                    "Error", MessageBoxButtons.OK);
                Application.Exit();
            }
        }

        private bool Run(ListBox.ObjectCollection logger)
        {
            this.logger = logger;

            switch (Command)
            {
                case Download:
                    if (File.Exists(CommandArg2))
                    {
                        Log($"Did not download {CommandArg1} as the target file already exists");
                        return true;
                    }
                    Log($"Downloading {CommandArg1} to {CommandArg2}");
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(new Uri(CommandArg1), CommandArg2);
                    }
                    Log($"Finished downloading {CommandArg1}");
                    break;
                case CreateFolder:
                    if (Directory.Exists(CommandArg1))
                    {
                        Log($"Did not create directory {CommandArg1} as it already exists");
                        return true;
                    }
                    Log($"Creating directory {CommandArg1}");
                    Directory.CreateDirectory(CommandArg1);
                    break;
                case RunProgram:
                    Log($"Running `{CommandArg1} {CommandArg2}`");
                    var start = new ProcessStartInfo
                    {
                        Arguments = CommandArg2,
                        FileName = CommandArg1,
                        WindowStyle = ProcessWindowStyle.Normal,
                        CreateNoWindow = false
                    };
                    using (var process = Process.Start(start))
                    {
                        process.WaitForExit();
                    }
                    Log($"Finished running {CommandArg1}");
                    break;
                case Copy:
                    if (File.Exists(CommandArg2))
                    {
                        Log($"Did not copy {CommandArg1} as the target file already exists");
                        return true;
                    }
                    Log($"Copying {CommandArg1} to {CommandArg2}");
                    File.Copy(CommandArg1, CommandArg2);
                    break;
                case Commands.Shortcut:
                    if (File.Exists(CommandArg2))
                    {
                        Log($"Did not create shortcut to {CommandArg1} as the target file already exists");
                        return true;
                    }
                    Log($"Creating shortcut from {CommandArg1} to {CommandArg2}");
                    CreateShortcut(CommandArg2, CommandArg1, "MiniMusic Editor");
                    break;
                case DeleteDirectory:
                    if (!Directory.Exists(CommandArg1))
                    {
                        Log($"Did not delete directory {CommandArg1} as it does not exist");
                        return true;
                    }
                    Log($"Deleting directory {CommandArg1} and all its subcontents");
                    new DirectoryInfo(CommandArg1).Delete(true);
                    break;
                case FileAssociation:
                    Log($"Creating file association for *{CommandArg1}");
                    var fileTypeKey = Registry.ClassesRoot.CreateSubKey(CommandArg1);
                    fileTypeKey.SetValue("", $"MiniMusic.FileType{CommandArg1}");
                    var fileDescKey = Registry.ClassesRoot.CreateSubKey($"MiniMusic.FileType{CommandArg1}");
                    fileDescKey.SetValue("", CommandArg2);
                    var fileIconKey = fileDescKey.CreateSubKey("DefaultIcon");
                    fileIconKey.SetValue("", CommandArg3 + ",0");
                    var shellKey = fileDescKey.CreateSubKey("shell");
                    var openCommand = shellKey.CreateSubKey("open\\command");
                    openCommand.SetValue("", $"\"{CommandArg3}\" -play \"%1\"");
                    var editCommand = shellKey.CreateSubKey("edit\\command");
                    editCommand.SetValue("", $"\"{CommandArg3}\" -edit \"%1\"");
                    break;
                case Commands.Delegate:
                    return CommandArg4();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        internal static void Run(IEnumerable<InstallationCommand> commands, ListBox.ObjectCollection logger)
        {
            var dlThread = new Thread(new ThreadStart(delegate
            {
                foreach (var command in commands)
                    if (!command.Run(logger)) break;
            }));
            dlThread.Start();
        }

        private static void CreateShortcut(string path, string target, string description)
        {
            var fileLocation = Form1.GeneratePath() + ".vbs";
            string[] lines =
            {
                "set WshShell = WScript.CreateObject(\"WScript.Shell\")",
                $"set oShellLink = WshShell.CreateShortcut(\"{path}\")",
                $"oShellLink.TargetPath = \"{target}\"",
                "oShellLink.WindowStyle = 1",
                $"oShellLink.IconLocation = \"{target}, 0\"",
                $"oShellLink.Description = \"{description}\"",
                "oShellLink.Save()"
            };
            File.WriteAllLines(fileLocation, lines);
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "cscript.exe",
                    Arguments = fileLocation
                }
            };
            process.Start();
            process.WaitForExit();
            File.Delete(fileLocation);
        }

        /*/// <summary>
        /// Creates a shortcut at the specified path with the given target and
        /// arguments.
        /// </summary>
        /// <param name="path">The path where the shortcut will be created. This should
        ///     be a file with the LNK extension.</param>
        /// <param name="target">The target of the shortcut, e.g. the program or file
        ///     or folder which will be opened.</param>
        /// <param name="arguments">The additional command line arguments passed to the
        ///     target.</param>
        public static void CreateShortcut(string path, string target, string arguments)
        {
            // Check if link path ends with LNK or URL
            string extension = Path.GetExtension(path).ToUpper();
            if (extension != ".LNK" && extension != ".URL")
            {
                throw new ArgumentException("The path of the shortcut must have the extension .lnk or .url.");
            }

            // Get temporary file name with correct extension
            _scriptTempFilename = Path.GetTempFileName();
            File.Move(_scriptTempFilename, _scriptTempFilename += ".vbs");

            // Generate script and write it in the temporary file
            File.WriteAllText(_scriptTempFilename, string.Format(@"Dim WSHShell
Set WSHShell = WScript.CreateObject({0}WScript.Shell{0})
Dim Shortcut
Set Shortcut = WSHShell.CreateShortcut({0}{1}{0})
Shortcut.TargetPath = {0}{2}{0}
Shortcut.WorkingDirectory = {0}{3}{0}
Shortcut.Arguments = {0}{4}{0}
Shortcut.Save",
                    "\"", path, target, Path.GetDirectoryName(target), arguments),
                Encoding.Unicode);

            // Run the script and delete it after it has finished
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "cscript.exe",
                    Arguments = $"/B /Nologo \"{_scriptTempFilename}\"",
                    Verb = "runas"
                }
            };
            process.Start();
            process.WaitForExit();
            File.Delete(_scriptTempFilename);
        }*/
    }
}