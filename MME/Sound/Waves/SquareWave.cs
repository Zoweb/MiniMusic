using System;

namespace MME.Sound.Waves
{
    internal class SquareWave : WaveFunction
    {
        public override float Amplitude { get; set; } = 0.25f;
        public override float Frequency { get; set; } = 1000;
        public override string Type { get; } = "SquareWave";

        public override float Get(int smpl, int smpr)
        {
            return Math.Sign(Amplitude * Math.Sin(2 * Math.PI * smpl * Frequency / smpr));
        }

        public override WaveFunction Clone()
        {
            var newAmp = Amplitude;
            var newFreq = Frequency;

            return new SquareWave
            {
                Amplitude = newAmp,
                Frequency = newFreq
            };
        }
    }
}