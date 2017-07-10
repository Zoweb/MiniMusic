namespace MME.Sound
{
    public abstract class WaveFunction
    {
        public abstract float Amplitude { get; set; }
        public abstract float Frequency { get; set; }
        public abstract string Type { get; }

        public abstract float Get(int smpl, int smpr);
        public abstract WaveFunction Clone();
    }
}