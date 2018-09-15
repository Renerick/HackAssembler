using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackAssembler.Core
{
    public class HackParser
    {
        private readonly Dictionary<string, string> _comps = new Dictionary<string, string>
        {
            ["0"] = "0101010",
            ["1"] = "0111111",
            ["-1"] = "0111010",
            ["D"] = "0001100",
            ["A"] = "0110000",
            ["M"] = "1110000",
            ["!D"] = "0001101",
            ["!A"] = "0110001",
            ["!M"] = "1110001",
            ["-D"] = "0001111",
            ["-A"] = "0110011",
            ["-M"] = "1110011",
            ["D+1"] = "0011111",
            ["A+1"] = "0110111",
            ["M+1"] = "1110111",
            ["D-1"] = "0001110",
            ["A-1"] = "0110010",
            ["M-1"] = "1110010",
            ["D+A"] = "0000010",
            ["D+M"] = "1000010",
            ["D-A"] = "0010011",
            ["D-M"] = "1010011",
            ["A-D"] = "0000111",
            ["M-D"] = "1000111",
            ["D&A"] = "0000000",
            ["D&M"] = "1000000",
            ["D|A"] = "0010101",
            ["D|M"] = "1010101"
        };

        private readonly Dictionary<string, string> _jumps = new Dictionary<string, string>
        {
            ["jgt"] = "001",
            ["jeq"] = "010",
            ["jge"] = "011",
            ["jlt"] = "100",
            ["jne"] = "101",
            ["jle"] = "110",
            ["jmp"] = "111"
        };

        private readonly SymbolsTable _symbols;
        private int _labelPosition;
        private int _linesCounter = -1;


        public HackParser()
        {
            _symbols = new SymbolsTable();
        }


        public IEnumerable<string> Parse(IEnumerable<string> code)
        {
            var enumerable = code.Select(_cleanLine).ToArray();

            _defineSymbols(enumerable);
            return enumerable
                .Select(_parseLine)
                .Where(r => r != null);
        }

        public string _parseLine(string line)
        {
            _linesCounter++;
            if (string.IsNullOrEmpty(line) || line.StartsWith("("))
                return null;


            var result = line.StartsWith("@") ? _parseACommand(line) : _parseCCommand(line);

            return result;
        }

        private void _defineSymbols(IEnumerable<string> code)
        {
            foreach (var line in code)
            {
                _linesCounter++;
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.StartsWith("("))
                    _parseLCommand(line);
                else
                    _labelPosition++;
            }
            _linesCounter = -1;
        }

        private string _cleanLine(string line)
        {
            var pos = line.IndexOf("//", StringComparison.Ordinal);
            return pos < 0 ? line.Trim() : line.Substring(0, pos).Trim();
        }


        #region Parsers

        private string _parseACommand(string command)
        {
            var value = command.Substring(1);

            var result = int.TryParse(value, out var address) ? address : _symbols.Get(value);
            
            var resultString = Convert.ToString(result, 2).PadLeft(15, '0');
            return $"0{resultString.Substring(resultString.Length - 15)}";
        }

        private string _parseCCommand(string command)
        {
            // remove whitespaces
            command = command.Replace(" ", "");

            var dest = new StringBuilder("000");
            var jump = "000";

            // build dest section
            var destLength = _buildDest(command, dest);

            // build jmp section
            var jumpLength = 0;
            if (command.Length > 4 && command[command.Length - 3 - 1] == ';')
            {
                jumpLength = 3;
                var jumpType = command.Substring(command.Length - jumpLength).ToLower();
                if (!_jumps.TryGetValue(jumpType, out jump))
                    throw new ParserException("Syntax error: unknown jump command");
                jumpLength = 4;
            }

            var compExp = command.Substring(destLength + 1, command.Length - jumpLength - (destLength + 1));

            if (!_comps.TryGetValue(compExp, out var comp))
                throw new ParserException($"Syntax error: unkown comp {compExp}", _linesCounter);

            return $"111{comp}{dest}{jump}";
        }

        private void _parseLCommand(string command)
        {
            if (!command.EndsWith(")"))
                throw new ParserException("Syntax error: unexpected end of line", _linesCounter);

            var name = command.Substring(1, command.Length - 2);
            try
            {
                _symbols.Add(name, _labelPosition);
            }
            catch (SymbolsException e)
            {
                throw new ParserException("Syntax error: duplicated lable declaration", _linesCounter, e);
            }
        }

        private int _buildDest(string command, StringBuilder dest)
        {
            var destLength = command.IndexOf('=');
            if (destLength < 0) return destLength;
            if (destLength == 0) throw new ParserException("Unexpected character \"=\"", _linesCounter);

            for (var i = 0; i < destLength; i++)
                switch (command[i])
                {
                    case 'A':
                        dest[0] = '1';
                        break;
                    case 'D':
                        dest[1] = '1';
                        break;
                    case 'M':
                        dest[2] = '1';
                        break;
                    default:
                        throw new ParserException("Syntax error: unknown dest", _linesCounter);
                }
            return destLength;
        }

        #endregion
    }
}
