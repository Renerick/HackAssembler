using System;
using System.Runtime.Serialization;

namespace HackAssembler.Core
{
    [Serializable]
    public class SymbolsException : Exception
    {
        public string Symbol;

        public SymbolsException()
        {
        }

        public SymbolsException(string message, string symbol) : base(message)
        {
            Symbol = symbol;
        }

        public SymbolsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SymbolsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}