using System;

namespace MME.Sound.Waves
{
    internal class SineWave : WaveFunction
    {
        public override float Amplitude { get; set; } = 0.25f;
        public override float Frequency { get; set; } = 1000;
        public override string Type { get; } = "SineWave";

        public override float Get(int smpl, int smpr)
        {
            return (float) (Amplitude * Math.Sin(2 * Math.PI * smpl * Frequency / smpr));
        }

        public override WaveFunction Clone()
        {
            var newAmp = Amplitude;
            var newFreq = Frequency;

            return new SineWave
            {
                Amplitude = newAmp,
                Frequency = newFreq
            };
        }
    }
}