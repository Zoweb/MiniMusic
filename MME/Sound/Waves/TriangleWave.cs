using System;

namespace MME.Sound.Waves
{
    internal class TriangleWave : WaveFunction
    {
        public override float Amplitude { get; set; } = 0.25f;
        public override float Frequency { get; set; } = 1000;
        public override string Type { get; } = "TriangleWave";

        public override float Get(int smpl, int smpr)
        {
            return (float) (Amplitude * Math.Asin(Math.Cos(2 * Math.PI * smpl * Frequency / smpr)) / 1.5708);
        }

        public override WaveFunction Clone()
        {
            var newAmp = Amplitude;
            var newFreq = Frequency;

            return new TriangleWave
            {
                Amplitude = newAmp,
                Frequency = newFreq
            };
        }
    }
}