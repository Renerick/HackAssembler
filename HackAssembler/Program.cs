using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HackAssembler.Core;

namespace HackAssembler
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Source path is not defined. Usage: HackAssembler.exe <source_path>");
                return 1;
            }
            var sourcePath = args[0];

            var parser = new HackParser();
            try
            {
                IEnumerable<string> lines;
                
                using (var source = new StreamReader(sourcePath))
                {
                    if (!sourcePath.ToLower().EndsWith(".asm"))
                        Console.WriteLine("File is not .asm file");
                    
                    lines = parser.Parse(source.ReadToEnd().Split(new[] {"\n", "\r\n"}, StringSplitOptions.None))
                        .ToArray();
                }
                
                var outPath = sourcePath.Replace(".asm", ".hack");
                using (var output = new StreamWriter(outPath))
                {
                    output.Write(string.Join("\n", lines));
                }
                
                Console.WriteLine("Compilation successful");
                Console.WriteLine($"Output location: {outPath}");
                return 0;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }
            catch (ParserException e)
            {
                Console.WriteLine($"{e.Message} at line {e.Line}");
                return 1;
            }
        }
    }
}