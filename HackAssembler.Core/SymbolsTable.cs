using System.Collections.Generic;

namespace HackAssembler.Core
{
    public class SymbolsTable
    {
        private readonly Dictionary<string, Symbol> _symbols;
        private int _startAddress = 0;

        public SymbolsTable()
        {
            _symbols = new Dictionary<string, Symbol>();
            _initializeBaseSymbols();
        }

        public void Reset()
        {
            _symbols.Clear();
            _initializeBaseSymbols();
        }

        public void Add(string name)
        {
            _addSymbol(name, _startAddress, false);
            _startAddress++;
        }

        public void Add(string name, int value)
        {
            _addSymbol(name, value, true);
        }

        private void _addSymbol(string name, int value, bool isConst)
        {
            if (_symbols.ContainsKey(name))
            {
                if (_symbols[name].IsLabel)
                {
                    throw new SymbolsException("Duplicated symbol declaration", name);
                }

                _symbols[name] = new Symbol(value, true);
                return;
            }
            _symbols.Add(name, new Symbol(value, isConst));
        }

        public int Get(string name)
        {
            if (_symbols.TryGetValue(name, out var result)) 
                return result.Value;

            Add(name);
            return _symbols[name].Value;
        }

        public int this[string index] => Get(index);

        private void _initializeBaseSymbols()
        {
            Add("R0", 0);
            Add("R1", 1);
            Add("R2", 2);
            Add("R3", 3);
            Add("R4", 4);
            Add("R5", 5);
            Add("R6", 6);
            Add("R7", 7);
            Add("R8", 8);
            Add("R9", 9);
            Add("R10", 10);
            Add("R11", 11);
            Add("R12", 12);
            Add("R13", 13);
            Add("R14", 14);
            Add("R15", 15);

            Add("SCREEN", 0x4000); // 0x4000
            Add("KBD", 0x6000); // 0x6000

            Add("SP", 0);
            Add("LCL", 1);
            Add("ARG", 2);
            Add("THIS", 3);
            Add("THAT", 4);

            _startAddress = 16;
        }

        private struct Symbol
        {
            public bool IsLabel { get; }
            public int Value { get; }

            public Symbol(int value, bool isLabel)
            {
                IsLabel = isLabel;
                Value = value;
            }
        }
    }
}