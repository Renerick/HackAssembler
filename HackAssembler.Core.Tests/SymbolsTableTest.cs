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
            Assert.Equal(16, table["Sym"]);
        }
        
        [Fact]
        public void AddAndGetLabelTest()
        {
            var table = new SymbolsTable();
            table.Add("Sym", 5);
            Assert.Equal(5, table["Sym"]);
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
            Assert.Equal(16, table.Get("Sym1"));
            table.Add("Sym1", 5);
            Assert.Equal(5, table.Get("Sym1"));
        }

        [Fact]
        public void GetUndeclaredTest()
        {
            var table = new SymbolsTable();
            Assert.Equal(16, table.Get("Sym1"));
            Assert.Equal(17, table.Get("Sym2"));
            Assert.Equal(16, table.Get("Sym1"));
        }
    }
}