using System;
using Xunit;

namespace FakeDb.Tests
{
    public class FakeDbFacts
    {
        public class MapId
        {
            [Fact]
            public void CanMapId()
            {
                var db = new Db();
                db.MapId((Car c) => c.CarId);
                
                var car = new Car();
                db.Set<Car>().Add(car);

                Assert.True(car.CarId != 0);
                Assert.True(car.Id == 0);
            }
        }

        public class MapForeignKey
        {
            [Fact]
            public void CanMapForeignKey()
            {
                var db = new Db();
                db.RegisterForeignKeyInitializer();
                db.MapForeignKey((Person c) => c._AddressId, c => c.Address);

                var p = new Person {Address = new Address()};

                db.Set<Person>().Add(p);
                Assert.NotNull(p._AddressId);
                Assert.Null(p.AddressId);
            }
        }
    }
}