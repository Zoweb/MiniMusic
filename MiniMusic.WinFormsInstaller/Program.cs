using System;
using System.Windows.Forms;

namespace MiniMusic.WinFormsInstaller
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var startatsecond = false;
            var startLocation = "";
            var skipStart = false;
            if (args.Length > 0)
                if (args[0] == "Start_At_Next_Section")
                {
                    startatsecond = true;
                    startLocation = args[1];
                    if (args[2] == "True") skipStart = true;
                }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form1(startatsecond, startLocation, skipStart);
            Form1.Instance = form;
            Application.Run(form);
        }
    }
}