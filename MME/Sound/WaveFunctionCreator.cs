namespace MME.Sound
{
    public class WaveFunctionCloner<T> where T : WaveFunction, new()
    {
        public static T Clone(T instrument)
        {
            var frequency = instrument.Frequency;
            var amplitude = instrument.Amplitude;

            return new T
            {
                Frequency = frequency,
                Amplitude = amplitude
            };
        }
    }
}