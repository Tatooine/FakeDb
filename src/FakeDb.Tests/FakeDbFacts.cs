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

                Assert.True(car.CarId > 0);
                Assert.True(car.Id == 0);
            }
        }
    }
}