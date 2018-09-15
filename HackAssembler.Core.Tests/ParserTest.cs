using System.Linq;
using HackAssembler.Core;
using Xunit;

namespace ParserTest
{
    public class ParserTest
    {
        [Theory]
        [InlineData("M = 0", "1110101010001000")]
        [InlineData("D=D +A", "1110000010010000")]
        [InlineData("M=D", "1110001100001000")]
        [InlineData("D;JGT", "1110001100000001")]
        [InlineData("0;JMP", "1110101010000111")]
        public void CCommandParseTest(string source, string result)
        {
            var parser = new HackParser();
            var hack = parser.Parse(new[] {source});
            Assert.Equal(hack, new []{result});
        }

        [Fact]
        public void ACommandParseTest()
        {
            var parser = new HackParser();
            var hack = parser.Parse(new[] {"@2"});
            Assert.Equal(hack, new [] {"0000000000000010"});
        }

        [Fact]
        public void ASymbolCommandParseTest()
        {
            var parser = new HackParser();
            var hack = parser.Parse(new[] {"@SYM"});
            Assert.Equal(hack, new [] {"0000000000010000"});
        }

        [Fact]
        public void SymbolPostDefinitionTest()
        {
            var parser = new HackParser();
            var hack = parser.Parse(new[] {"@SYM", "0", "(SYM)"});
            Assert.Equal(hack.First(), "0000000000000010");
        }

        [Fact]
        public void SymbolPreDefinitionTest()
        {
            var parser = new HackParser();
            var hack = parser.Parse(new[] {"0", "(SYM)", "@SYM"});
            Assert.Equal(hack.Last(), "0000000000000001");
        }
    }
}