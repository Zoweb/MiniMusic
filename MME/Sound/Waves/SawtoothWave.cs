using System;

namespace MME.Sound.Waves
{
    internal class SawtoothWave : WaveFunction
    {
        public override float Amplitude { get; set; } = 0.25f;
        public override float Frequency { get; set; } = 1000;
        public override string Type { get; } = "SawtoothWave";

        public override float Get(int smpl, int smpr)
        {
            return (float) (Amplitude * (2 * Math.PI * smpl * Frequency / smpr));
        }

        public override WaveFunction Clone()
        {
            var newAmp = Amplitude;
            var newFreq = Frequency;

            return new SawtoothWave
            {
                Amplitude = newAmp,
                Frequency = newFreq
            };
        }
    }
}