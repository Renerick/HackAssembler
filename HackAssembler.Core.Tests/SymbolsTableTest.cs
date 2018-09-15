using Xunit;

namespace HackAssembler.Core.Tests
{
    public class SymbolsTableTest
    {
        [Fact]
        public void AddAndGetSymbolTest()
        {
            var table = new SymbolsTable();
            table.Add("Sym");
            Assert.Equal(table.Get("Sym"), 16);
        }
        
        [Fact]
        public void AddAndGetLabelTest()
        {
            var table = new SymbolsTable();
            table.Add("Sym", 5);
            Assert.Equal(table.Get("Sym"), 5);
        }

        [Fact]
        public void AddDuplicatedLabelTest()
        {
            var table = new SymbolsTable();
            table.Add("Sym", 4);

            Assert.Throws<SymbolsException>(() => table.Add("Sym", 5));
        }

        [Fact]
        public void GetUndeclaredAndAddTest()
        {
            var table = new SymbolsTable();
            Assert.Equal(table.Get("Sym1"), 16);
            table.Add("Sym1", 5);
            Assert.Equal(table.Get("Sym1"), 5);
        }

        [Fact]
        public void GetUndeclaredTest()
        {
            var table = new SymbolsTable();
            Assert.Equal(table.Get("Sym1"), 16);
            Assert.Equal(table.Get("Sym2"), 17);
            Assert.Equal(table.Get("Sym1"), 16);
        }
    }
}