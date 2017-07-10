namespace MiniMusic.Installer.Version2
{
    internal class LoggerOutputEventArgs
    {
        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                var split = value.Split(';');
                Command = split[0];
                Id = split[1];
                if (split.Length > 2) Value = split[2];
            }
        }

        public string Command { get; private set; }

        public string Value { get; private set; }

        public string Id { get; private set; }
    }
}