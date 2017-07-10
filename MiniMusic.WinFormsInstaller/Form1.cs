using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace MiniMusic.WinFormsInstaller
{
    public partial class Form1 : Form
    {
        public static Form Instance;
        public static ListBox Logger;

        private static string tempDir;

        private int _clickIndex;

        public Form1(bool startAtSecond, string startLocation, bool skipStart)
        {
            InitializeComponent();

            Logger = listBox1;

            Height = 172;
            button2.Location = new Point(12, 97);
            groupBox2.Visible = false;
            listBox1.Visible = false;

            if (startAtSecond)
            {
                groupBox3.Visible = false;

                Location.Text = startLocation;
                groupBox2.Visible = true;
                _clickIndex = 1;
            }
            if (skipStart)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                if (!CheckIfAdmin())
                {
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                }
                listBox1.BringToFront();
                Install();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "MiniMusic Editor");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*var folderBrowser = new FolderBrowserDialogEx
            {
                Description = "Select an installation folder",
                ShowNewFolderButton = true,
                ShowEditBox = true,
                SelectedPath = Location.Text,
                ShowFullPathInEditBox = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            var result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
                Location.Text = Path.Combine(folderBrowser.SelectedPath, "MiniMusic Editor");*/
            Location.Text = Interaction.InputBox("input file path", "this is just for testing cuz the thing wont open for some reason", Location.Text);
        }

        private bool CheckIfAdmin(string path = null)
        {
            if (path == null) path = Path.Combine(Environment.GetEnvironmentVariable("programfiles"), "test");

            try
            {
                Directory.CreateDirectory(path);

                try
                {
                    Directory.Delete(path);
                    return true;
                }
                catch (IOException)
                {
                    return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _clickIndex++;
            switch (_clickIndex)
            {
                case 1:
                    if (radioButton1.Checked)
                        if (!CheckIfAdmin(Location.Text))
                            InstallationCommand.StartUAC(Location.Text, true);
                        else
                        {

                            button2_Click(sender, e);
                            button2_Click(sender, e);
                        }
                    groupBox3.Visible = false;
                    break;
                case 2:
                    // See if we need admin privs to do stuff here
                    groupBox2.Visible = true;
                    groupBox2.BringToFront();
                    groupBox3.Visible = false;
                    if (!CheckIfAdmin())
                    {
                        checkBox2.Checked = false;
                        checkBox2.Enabled = false;
                        checkBox3.Checked = false;
                        checkBox3.Enabled = false;
                    }
                    if (!CheckIfAdmin(Location.Text))
                        InstallationCommand.StartUAC(Location.Text);


                    break;
                case 3:
                    if (!CheckIfAdmin())
                    {
                        checkBox2.Checked = false;
                        checkBox3.Checked = false;
                    }
                    Install();
                    break;
                case 4:
                    Close();
                    break;
            }
        }

        private static string GenerateUuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GeneratePath()
        {
            return Path.Combine(tempDir, GenerateUuid());
        }

        private void Install()
        {
            button2.Text = "Exit";
            button2.Enabled = false;
            listBox1.Visible = true;
            listBox1.TopIndex = Math.Max(listBox1.Items.Count - listBox1.ClientSize.Height / listBox1.ItemHeight + 1,
                0);

            tempDir = Path.Combine(Environment.GetEnvironmentVariable("temp"),
                "MiniMusicInstallerTemp_" + GenerateUuid());

            var nAudioTemp = GeneratePath();
            var sharpZipLibTemp = GeneratePath();
            var wpfToolkitTemp = GeneratePath();
            var inputToolkitTemp = GeneratePath();
            var layoutToolkitTemp = GeneratePath();
            var miniMusicTemp = GeneratePath();
            var licenseTemp = Path.Combine(Location.Text, ".license.exe");
            var licenseCorrectTemp = GeneratePath();

            var commands = new List<InstallationCommand>
            {
                // Create installation folder
                new InstallationCommand(InstallationCommand.Commands.CreateFolder, Location.Text),
                // Create temp folder
                new InstallationCommand(InstallationCommand.Commands.CreateFolder, tempDir),

                // Download NAudio
                new InstallationCommand(InstallationCommand.Commands.Download,
                    "http://download-codeplex.sec.s-msft.com/Download/Release?ProjectName=naudio&DownloadId=1626436&FileTime=131273413931730000&Build=21050",
                    nAudioTemp + ".zip"),
                // Unzip NAudio
                new InstallationCommand(InstallationCommand.Commands.RunProgram, "powershell.exe",
                    $"-nologo -noprofile -command \"& {{ Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('{nAudioTemp}.zip', '{nAudioTemp}'); }}\""),
                // Copy DLL to installation directory
                new InstallationCommand(InstallationCommand.Commands.Copy, nAudioTemp + "/NAudio.dll",
                    Path.Combine(Location.Text, "NAudio.dll")),

                // Download SharpZipLib
                new InstallationCommand(InstallationCommand.Commands.Download,
                    "https://downloads.sourceforge.net/project/sharpdevelop/SharpZipLib/0.86/SharpZipLib_0860_Bin.zip?r=http%3A%2F%2Fwww.icsharpcode.net%2FOpenSource%2FSharpZipLib%2FDownload.aspx&ts=1495787198&use_mirror=nchc",
                    sharpZipLibTemp + ".zip"),
                // Unzip SharpZipLib
                new InstallationCommand(InstallationCommand.Commands.RunProgram, "powershell.exe",
                    $"-nologo -noprofile -command \"& {{ Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('{sharpZipLibTemp}.zip', '{sharpZipLibTemp}'); }}\""),
                // Copy DLL to installation directory
                new InstallationCommand(InstallationCommand.Commands.Copy,
                    sharpZipLibTemp + "/net-11/ICSharpCode.SharpZipLib.dll",
                    Path.Combine(Location.Text, "ICSharpCode.SharpZipLib.dll")),

                // Download WPF
                new InstallationCommand(InstallationCommand.Commands.Download,
                    "http://code.zoweb.me/mini-music/installer/WPFToolkit.dll", wpfToolkitTemp),
                new InstallationCommand(InstallationCommand.Commands.Download,
                    "http://code.zoweb.me/mini-music/installer/System.Windows.Controls.Input.Toolkit.dll",
                    inputToolkitTemp),
                new InstallationCommand(InstallationCommand.Commands.Download,
                    "http://code.zoweb.me/mini-music/installer/System.Windows.Controls.Layout.Toolkit.dll",
                    layoutToolkitTemp),
                // And copy it
                new InstallationCommand(InstallationCommand.Commands.Copy, wpfToolkitTemp,
                    Path.Combine(Location.Text, "WPFToolkit.dll")),
                new InstallationCommand(InstallationCommand.Commands.Copy, inputToolkitTemp,
                    Path.Combine(Location.Text, "System.Windows.Controls.Input.Toolkit.dll")),
                new InstallationCommand(InstallationCommand.Commands.Copy, layoutToolkitTemp,
                    Path.Combine(Location.Text, "System.Windows.Controls.Layout.Toolkit.dll")),

                // Download license display
                new InstallationCommand(InstallationCommand.Commands.Download,
                    "http://code.zoweb.me/mini-music/installer/MiniMusic.License.exe", licenseTemp),
                new InstallationCommand(InstallationCommand.Commands.RunProgram, licenseTemp, licenseCorrectTemp),
                new InstallationCommand(InstallationCommand.Commands.Delegate, null, null, null, delegate
                {

                    var fileExists = File.Exists(licenseCorrectTemp);

                    if (!fileExists)
                    {
                        Directory.Delete(Location.Text, true);
                        Directory.Delete(tempDir, true);
                        MessageBox.Show("You must agree to the licenses to install MiniMusic. You will need to re-run the installer if you want to continue.");
                        Application.Exit();
                        return false;
                    }

                    File.Delete(licenseCorrectTemp);
                    return true;
                }),
                new InstallationCommand(InstallationCommand.Commands.Download,
                    "http://code.zoweb.me/mini-music/installer/MiniMusic.exe", miniMusicTemp),
                new InstallationCommand(InstallationCommand.Commands.Copy, miniMusicTemp,
                    Path.Combine(Location.Text, "MiniMusic.exe"))
            };


            // Download MiniMusic

            if (checkBox1.Checked)
                commands.Add(new InstallationCommand(InstallationCommand.Commands.Shortcut,
                    Path.Combine(Location.Text, "MiniMusic.exe"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"Microsoft\Windows\Start Menu\Programs\MiniMusic.lnk")));
            if (checkBox2.Checked)
                commands.Add(new InstallationCommand(InstallationCommand.Commands.Shortcut,
                    Path.Combine(Location.Text, "MiniMusic.exe"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                        @"MiniMusic.lnk")));

            if (checkBox3.Checked)
                commands.Add(new InstallationCommand(InstallationCommand.Commands.FileAssociation, ".mmx",
                    "MiniMusic file", Path.Combine(Location.Text, "MiniMusic.exe")));

            commands.Add(new InstallationCommand(InstallationCommand.Commands.DeleteDirectory, tempDir));

            commands.Add(new InstallationCommand(InstallationCommand.Commands.Delegate, null, null, null, delegate
            {
                button2.Enabled = true;

                File.Delete(licenseTemp);

                Logger.Items.Add("Done!");

                return false;
            }));

            InstallationCommand.Run(commands, listBox1.Items);
        }
    }
}