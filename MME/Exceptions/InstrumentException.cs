using System;

namespace MME.Exceptions
{
    public class InstrumentException : Exception
    {
        public InstrumentException()
        {
        }

        public InstrumentException(string message) : base(message)
        {
        }

        public InstrumentException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}