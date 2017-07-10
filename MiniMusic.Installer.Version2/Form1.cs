using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniMusic.WinFormsInstaller;

namespace MiniMusic.Installer.Version2
{
    public partial class MainWindow : Form
    {

        private StreamWriter _processStandardInput;
        private StreamReader _processStandardOutput;
        private bool _hasExtractorStarted;

        private Point _groupPoint = new Point(12, 55);

        private string _saveLocation =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "MiniMusic Editor");

        private enum Stages
        {
            CheckAdmin,
            SetLocation,
            CheckLocation,
            Install
        }

        private delegate void LogEventHandler(object sender, LoggerOutputEventArgs e);

        private event LogEventHandler logEvent;

        private void RunExtractor(bool runAsAdministrator = false)
        {
            if (runAsAdministrator)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    Verb = "runas"
                });

                Process.GetCurrentProcess().Kill();

                return;
            }

            var extractor = new Process {
                StartInfo = {
                    FileName = "MiniMusic.Installer.Version2.Extractor.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            extractor.Start();

            Console.WriteLine($"Started extractor process. Process ID = {extractor.Id}");

            var extractorThread = new Thread(() =>
            {
                Console.WriteLine("Started extractor thread. ");
                
                _processStandardInput = extractor.StandardInput;

                _hasExtractorStarted = true;

                while (!extractor.HasExited)
                {
                    var line = extractor.StandardOutput.ReadLine();
                    if (line == null) continue;;
                }
            });

            extractorThread.Start();
        }

        public MainWindow()
        {
            InitializeComponent();

            Width = 442;
            Height = 171;

            runAsAdministratorPanel.Top = _groupPoint.Y;
            runAsAdministratorPanel.Left = _groupPoint.X;
            runAsAdministratorPanel.Visible = false;

            setLocationPanel.Top = _groupPoint.Y;
            setLocationPanel.Left = _groupPoint.X;
            setLocationPanel.Visible = false;

            checkLocationPanel.Top = _groupPoint.Y;
            checkLocationPanel.Left = _groupPoint.X;
            checkLocationPanel.Visible = false;

            installLogPanel.Top = _groupPoint.Y;
            installLogPanel.Left = _groupPoint.X;
            installLogPanel.Visible = false;

            if (IsUserAdmin())
            {
                RunExtractor();
                SetStage(Stages.SetLocation);
            }
            else
            {
                SetStage(Stages.CheckAdmin);
            }
        }

        private static bool IsUserAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void SetVisibleGroup(Stages stage)
        {
            runAsAdministratorPanel.Visible = false;
            setLocationPanel.Visible = false;
            checkLocationPanel.Visible = false;
            installLogPanel.Visible = false;

            switch (stage)
            {
                case Stages.CheckAdmin:
                    runAsAdministratorPanel.Visible = true;
                    Height = 104 + runAsAdministratorPanel.Height;
                    break;
                case Stages.SetLocation:
                    setLocationPanel.Visible = true;
                    Height = 104 + setLocationPanel.Height;
                    break;
                case Stages.CheckLocation:
                    checkLocationPanel.Visible = true;
                    Height = 104 + checkLocationPanel.Height;
                    break;
                case Stages.Install:
                    installLogPanel.Visible = true;
                    Height = 104 + installLogPanel.Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
            }
        }


        private void SetStage(Stages stage)
        {
            SetVisibleGroup(stage);

            switch (stage)
            {
                case Stages.CheckAdmin:
                    
                    break;
                case Stages.SetLocation:

                    break;
                case Stages.CheckLocation:
                    var driveData = new DriveInfo(Path.GetPathRoot(_saveLocation));
                    var freeSpace = driveData.TotalFreeSpace;
                    var formattedFreeSpace = string.Format(new FileSizeFormatProvider(), "{0:fs}", freeSpace);
                    locationAndSpace.Text = $@"Install to {_saveLocation}
({formattedFreeSpace} left)";
                    break;
                case Stages.Install:
                    Title.Text = "Installing...";

                    Install();

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
            }
        }

        private void CopyFile(string old, string path)
        {
            var actualPath = Path.Combine(_saveLocation, path);
            if (File.Exists(actualPath))
            {
                var messageBox = MessageBox.Show("The file " + path + " already exists. Do you want to continue?", "File Already Exists", MessageBoxButtons.YesNo);
                if (messageBox == DialogResult.Yes) File.Delete(actualPath);
                else return;
            }

            File.Copy(old, actualPath);
        }

        private async void Install()
        {
            // DOWNLOAD
            logOutput.Items.Add("Downloading...");
            var downloads = new List<FileDownload>
            {
                new FileDownload("http://code.zoweb.me/mini-music/dlls/sharp-zip-lib.dll", "NAudio.dll"),
                new FileDownload("http://code.zoweb.me/mini-music/dlls/naudio.dll", "ICSharpCode.SharpZipLib.dll")
            };

            await Task.WhenAll(downloads.Select(DownloadFile));
        }

        private async Task DownloadFile(FileDownload file)
        {
            var commandId = SendMessage("download", file.URL);

            LogEventHandler eventDelegate = null;

            eventDelegate = (sender, e) =>
            {
                if (e.Command != "response from download") return;
                if (e.Id != commandId) return;

                CopyFile(e.Value, file.Path);
                logOutput.Items.Add("Downloaded: " + file.Path);

                logEvent -= eventDelegate;
            };

            logEvent += eventDelegate;

            var location = await _processStandardOutput.ReadLineAsync();
            CopyFile(location, file.Path);
            logOutput.Items.Add("Downloaded " + file.Path);
        }

        private string SendMessage(string command, string message)
        {
            if (!_hasExtractorStarted) return null;
            var guid = Guid.NewGuid();
            var messageId = guid.ToString();

            _processStandardInput.WriteLine(command + ";" + messageId + ";" + message);

            return messageId;
        }

        private void runAsAdmin_Click(object sender, EventArgs e)
        {
            RunExtractor(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetStage(Stages.SetLocation);
            RunExtractor();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SendMessage("exit", "");
        }

        private void locationUsual_Click(object sender, EventArgs e)
        {
            _saveLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "MiniMusic Editor");
            SetStage(Stages.CheckLocation);
        }

        private void locationOk_Click(object sender, EventArgs e)
        {
            SendMessage(@"create folder", _saveLocation);
            SetStage(Stages.Install);
        }

        private void locationBad_Click(object sender, EventArgs e)
        {
            SetStage(Stages.SetLocation);
        }

        private void locationCalculate_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialogEx
            {
                ShowFullPathInEditBox = true,
                ShowEditBox = true,
                Description = "Select Installation Location",
                ShowNewFolderButton = true,
                SelectedPath = _saveLocation
            };
            var result = folderBrowser.ShowDialog();

            if (result != DialogResult.OK) return;
            _saveLocation = folderBrowser.SelectedPath;
            SetStage(Stages.CheckLocation);
        }
    }
}