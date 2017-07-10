using MME.Sound;

namespace MME.Data
{
    public class InstrumentData
    {
        public WaveFunction InstrumentFunction;
        public string InstrumentName;

        public InstrumentData()
        {
        }

        public InstrumentData(string name, WaveFunction fn)
        {
            InstrumentFunction = fn;
            InstrumentName = name;
        }
    }
}