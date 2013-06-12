using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace FakeDb.Tests
{
    public class ObjectGraphFacts
    {
        public class TraverseMethod
        {
            [Fact]
            public void ReturnsTheRoot()
            {
                var i = new {};
                var list = new ObjectGraph().Traverse(i);
                Assert.Contains(i, list);
            }

            [Fact]
            public void ReturnsChildObjects()
            {
                var child = new object();
                var parent = new {child};

                var list = new ObjectGraph().Traverse(parent).ToArray();

                Assert.Contains(child, list);
                Assert.Contains(parent, list);
            }

            [Fact]
            public void HandlesCyclicReferences()
            {
                var v = new Url("a");
                v.OutboundLinks.Add(v);

                var r = new ObjectGraph().Traverse(v).ToArray();

                Assert.Equal(1, r.Length);
                Assert.Contains(v, r);
            }
        }
    }

    public class Url
    {
        public string Link { get; set; }

        public Url(string link)
        {
            Link = link;
            OutboundLinks = new Collection<Url>();
        }

        public ICollection<Url> OutboundLinks { get; set; }
    }

}