using Xunit;

namespace FakeDb.Tests
{
    public class ForeignKeyFinderFacts
    {
        public class FindMethod
        {
            [Fact]
            public void ReturnsDefaultFkProperty()
            {
                var finder = new ForeignKeyFinderFinder();
                var prop = finder.Find(typeof (Person).GetProperty("Address"));

                Assert.Equal("AddressId", prop.Name);
            }

            [Fact]
            public void ReturnsRegisteredFkProperty()
            {
                var finder = new ForeignKeyFinderFinder();
                finder.RegisterForeignKey(typeof(Person), "_AddressId", "Address");

                Assert.Equal("_AddressId", finder.Find(typeof(Person).GetProperty("Address")).Name);
            }
        }
    }
}