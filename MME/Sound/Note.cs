using System;

namespace MME.Sound
{
    public class Note
    {
        public int Index;
        public float Volume;

        public float EndBeat
        {
            get => StartBeat + LengthBeats;
            set => LengthBeats = value - StartBeat;
        }

        private float _lengthBeats;
        public float LengthBeats
        {
            get => _lengthBeats;
            set => _lengthBeats = Math.Max((float) 1/8, value);
        }

        private float _startBeat;

        public float StartBeat
        {
            get => _startBeat;
            set => _startBeat = Math.Max(0, value);
        }

        private readonly Guid _noteGuid = new Guid();

        public Note()
        {
        }

        public Note(int index, float vol, float start, float length)
        {
            Index = index;
            Volume = vol;
            StartBeat = start;
            LengthBeats = length;
        }

        public override string ToString()
        {
            return $"{Index}@{Volume*100}%/{StartBeat}+{LengthBeats}";
        }

        public void ReplaceWith(Note note)
        {
            Index = note.Index;
            Volume = note.Volume;
            StartBeat = note.StartBeat;
            LengthBeats = note.LengthBeats;
        }

        public override int GetHashCode()
        {
            return _noteGuid.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other?.GetHashCode() == GetHashCode();
        }
    }
}