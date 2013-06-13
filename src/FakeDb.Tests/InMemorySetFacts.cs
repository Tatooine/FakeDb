using System;
using System.Linq;
using NSubstitute;
using Xunit;

namespace FakeDb.Tests
{
    public class InMemorySetFacts
    {
        public class AddMethod
        {
            [Fact]
            public void CanAddSimpleObject()
            {
                var fixture = new InMemorySetFixture();

                var obj = new Address();
                fixture.InMemorySet.Add(obj);

                Assert.Equal(1, fixture.InMemorySet.Items.Count);
                Assert.Equal(obj, fixture.InMemorySet.Items.First());
            }

            [Fact]
            public void CanAddSameObjectOnlyOnce()
            {
                var fixture = new InMemorySetFixture();

                var obj = new { };
                fixture.InMemorySet.Add(obj);
                fixture.InMemorySet.Add(obj);
                fixture.InMemorySet.Add(obj);

                Assert.Equal(obj, fixture.InMemorySet.Items.Single());
            }

            [Fact]
            public void CanAddNestedObject()
            {
                var fixture = new InMemorySetFixture();

                var person = new Person();
                var address = new Address();
                person.Address = address;

                fixture.InMemorySet.Add(person);

                Assert.Equal(person, fixture.InMemorySet.Items.Single());
                Assert.Equal(address, fixture.Cache.For(typeof(Address)).Items.Single());
            }

            [Fact]
            public void ThrowsNullArgExceptionWhenAddNullObject()
            {
                var fixture = new InMemorySetFixture();

                Assert.Throws<ArgumentNullException>(() => fixture.InMemorySet.Add(null));
            }

            [Fact]
            public void InvokesIdGeneratorForAllObjectsInGraph()
            {
                var person = new Person();
                var address = new Address();
                person.Address = address;

                var fixture = new InMemorySetFixture();

                fixture.InMemorySet.Add(person);

                fixture.IdGenerator.Received(1).Identify(person);
                fixture.IdGenerator.Received(1).Identify(address);
            }
        }

        public class RemoveMethod
        {
            [Fact]
            public void CanRemoveExistingObject()
            {
                var fixture = new InMemorySetFixture();
                var obj = new {};

                fixture.InMemorySet.Add(obj);
                fixture.InMemorySet.Remove(obj);

                Assert.Equal(0, fixture.InMemorySet.Items.Count);
            }
        }

        class InMemorySetFixture
        {
            public IIdGenerator IdGenerator { get; private set; }
            public ObjectGraph ObjectGraph { get; private set; }
            public Cache Cache { get; private set; }
            public InMemorySet InMemorySet { get; private set; }

            public InMemorySetFixture()
            {
                IdGenerator = Substitute.For<IIdGenerator>();
                IdGenerator.Identify(null).ReturnsForAnyArgs(c => c.Args()[0]);

                ObjectGraph = new ObjectGraph();
                Cache = new Cache(IdGenerator, ObjectGraph);
                InMemorySet = new InMemorySet(IdGenerator, Cache, ObjectGraph);
            }
        }
    }
}