using Xunit;

namespace FakeDb.Tests
{
    public class IdPropertyFinderFacts
    {
        public class FindMethod
        {
            [Fact]
            public void ReturnsDefaultIdProperty()
            {
                var finder = new IdPropertyFinder();
                var prop = finder.Find(typeof (Car));

                Assert.Equal(IdPropertyFinder.DefaultIdName, prop.Name);
            }

            [Fact]
            public void ReturnsRegisteredIdProperty()
            {
                var finder = new IdPropertyFinder();
                finder.RegisterIdName(typeof(Car), "CarId");
                var prop = finder.Find(typeof(Car));

                Assert.Equal("CarId", prop.Name);
            }

            [Fact]
            public void ShouldNotThrowExceptionEvenIfPropertyNotExist()
            {
                var finder = new IdPropertyFinder();
                finder.RegisterIdName(typeof(Car), "WrongId");
                var prop = finder.Find(typeof(Car));

                Assert.Equal("Id", prop.Name);
            }

            [Fact]
            public void CanHandleNoIdType()
            {
                var finder = new IdPropertyFinder();
                var prop = finder.Find(typeof(User));

                Assert.Null(prop);
            }
        }
    }
}