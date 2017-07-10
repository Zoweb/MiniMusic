using System;

namespace MME.Timers
{
    public class Timer
    {
        public delegate void ElapsedEventHandler(object sender);

        public event ElapsedEventHandler Elapsed;

        public bool Enabled;

        private long _interval;
        public long Interval
        {
            get => _interval;
            set
            {
                _interval = value;

                _timer.Change(value, value);
            }
        }

        private System.Threading.Timer _timer;

        private void _init()
        {
            _timer = new System.Threading.Timer(delegate
            {
                if (Enabled) Elapsed?.Invoke(this);
            });
        }

        public Timer()
        {
            _init();
        }

        public Timer(long interval)
        {
            Interval = interval;
            _init();
        }

        public Timer(double interval)
        {
            Interval = (long) interval;
            _init();
        }

        public void Start()
        {
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }

    public class ElapsedEventArgs : EventArgs
    {
        public DateTime SignalTime { get; }

        internal ElapsedEventArgs(int low, int high)
        {
            SignalTime = DateTime.FromFileTime((long)high << 32 | low & uint.MaxValue);
        }
    }
}