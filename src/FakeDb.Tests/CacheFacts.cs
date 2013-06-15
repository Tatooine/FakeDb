using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FakeDb.Tests
{
    public class CacheFacts
    {
        public class ForMethod
        {
            [Fact]
            public void ReturesEmptySet()
            {
                var fixture = new CacheFixture();

                var set = fixture.Cache.For(typeof (object));

                Assert.Equal(0, set.Items.Count);
            }
        }

        class CacheFixture
        {
            public IdGenerator IdGenerator { get; private set; }
            public ObjectGraph ObjectGraph { get; private set; }
            public Cache Cache { get; private set; }

            public IEnumerable<IMaterializationHook> MaterializationHooks {get;private set;}

            public CacheFixture()
            {
                IdGenerator = new IdGenerator();
                ObjectGraph = new ObjectGraph();
                MaterializationHooks = Enumerable.Empty<IMaterializationHook>();

                Cache = new Cache(IdGenerator, ObjectGraph, MaterializationHooks);
            }
        }
    }
}