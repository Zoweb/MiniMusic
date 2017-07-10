namespace MME.Sound
{
    public class NoteAndInstrument : Note
    {
        public readonly string InstrumentName;
        public WaveGenerator Voice;

        private readonly Note _original;

        public NoteAndInstrument(Note from)
        {
            StartBeat = from.StartBeat;
            LengthBeats = from.LengthBeats;
            Index = from.Index;
            Volume = from.Volume;

            _original = from;
        }

        public NoteAndInstrument(string instrumentName, Note from)
        {
            StartBeat = from.StartBeat;
            LengthBeats = from.LengthBeats;
            Index = from.Index;
            Volume = from.Volume;
            InstrumentName = instrumentName;

            _original = from;
        }

        public void UpdateOriginal()
        {
            _original.ReplaceWith(_original);
        }
    }
}