using System;
using System.Runtime.Serialization;

namespace HackAssembler.Core
{
    [Serializable]
    public class ParserException : Exception
    {
        public ParserException()
        {
        }

        public ParserException(string message) : base(message)
        {
        }

        public ParserException(string message, int row) : base(message)
        {
            Line = row;
        }


        public ParserException(string message, int row, Exception inner) : base(message, inner)
        {
            Line = row;
        }

        protected ParserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public int Line { get; }
    }
}