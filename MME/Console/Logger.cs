using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MME.Console
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Severe
    }

    public static class Logger
    {
        private static RichTextBox _loggerBox;
        private static Paragraph _paragraph;

        public static void Init(RichTextBox loggerBox)
        {
            _loggerBox = loggerBox;
            _paragraph = new Paragraph();
            _loggerBox.Document = new FlowDocument(_paragraph);
        }

        public static void Log(LogLevel logLevel, object message)
        {
            if (message == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                var type = DateTime.Now.ToString("HH:mm:ss tt") + " [";
                switch (logLevel)
                {
                    case LogLevel.Debug:
                        type += "debug";
                        break;
                    case LogLevel.Info:
                        type += "info";
                        break;
                    case LogLevel.Warning:
                        type += "warn";
                        break;
                    case LogLevel.Error:
                        type += "error";
                        break;
                    case LogLevel.Severe:
                        type += "severe";
                        break;
                }
                type += "]: ";

                var boldText = new TextBlock(new Run(type))
                {
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 170, 222))
                };

                _paragraph.Inlines.Add(boldText);
                _paragraph.Inlines.Add(new Bold(new Run(message.ToString())));
                _paragraph.Inlines.Add(new LineBreak());

                _loggerBox.ScrollToEnd();

                System.Console.WriteLine(type + message);
            });
        }

        public static void Console(LogLevel logLevel, object message)
        {
            if (message == null) return;

            var type = DateTime.Now.ToString("HH:mm:ss tt") + " [";
            switch (logLevel)
            {
                case LogLevel.Debug:
                    type += "debug";
                    break;
                case LogLevel.Info:
                    type += "info";
                    break;
                case LogLevel.Warning:
                    type += "warn";
                    break;
                case LogLevel.Error:
                    type += "error";
                    break;
                case LogLevel.Severe:
                    type += "severe";
                    break;
            }
            type += "]: ";

            System.Console.WriteLine(type + message);
        }

        public static void Debug(object message)
        {
            Log(LogLevel.Debug, message);
        }

        public static void Info(object message)
        {
            Log(LogLevel.Info, message);
        }

        public static void Warn(object message)
        {
            Log(LogLevel.Warning, message);
        }

        public static void Error(object message)
        {
            Log(LogLevel.Error, message);
        }

        public static void Severe(object message)
        {
            Log(LogLevel.Severe, message);
        }
    }
}