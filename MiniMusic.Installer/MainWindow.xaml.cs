using System;
using System.Windows;
using System.Windows.Forms;

namespace MiniMusic.Installer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileLocation_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}