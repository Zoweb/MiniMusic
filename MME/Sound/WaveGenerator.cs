using System.Threading;
using NAudio.Wave;

namespace MME.Sound
{
    public class WaveGenerator : WaveProvider32
    {
        private readonly WaveFunction _fn;
        private int _sample;

        private WaveOut _waveOut;

        public WaveGenerator(WaveFunction fn)
        {
            _fn = fn;
            SetWaveFormat(48000, 1);
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            var sampleRate = WaveFormat.SampleRate;
            for (var n = 0; n < sampleCount; n++)
            {
                buffer[n + offset] = _fn.Get(_sample, sampleRate);
                _sample++;
                if (_sample >= sampleRate) _sample = 0;
            }

            return sampleCount;
        }

        public void Play()
        {
            if (_waveOut != null)
                return;

            new Thread(() =>
            {
                _waveOut = new WaveOut();
                _waveOut.Init(this);
                _waveOut.Play();
            }).Start();
        }

        public void Stop()
        {
            if (_waveOut == null)
                return;

            _waveOut.Stop();
            _waveOut = null;
        }
    }
}