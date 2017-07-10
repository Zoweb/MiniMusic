using System.Windows.Input;

namespace MME
{
    public static class Commands
    {
        public static readonly RoutedCommand SaveCommand = new RoutedCommand();
        public static readonly RoutedCommand SaveAsCommand = new RoutedCommand();
        public static readonly RoutedCommand OpenCommand = new RoutedCommand();
    }
}