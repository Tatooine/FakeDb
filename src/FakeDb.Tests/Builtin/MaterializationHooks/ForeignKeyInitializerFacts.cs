using FakeDb.Builtin.MaterializationHooks;
using Xunit;

namespace FakeDb.Tests.Builtin.MaterializationHooks
{
    public class ForeignKeyInitializerFacts
    {
        public class ExecuteMethod
        {
            [Fact]
            public void SetsForeignKey()
            {
                var message = new Message
                    {
                        Id = 1,
                        Sender = new Sender
                            {
                                Id = 2
                            }
                    };

                new ForeignKeyInitializer().Execute(message);

                Assert.Equal(2, message.SenderId);
            }
        }

        public class Message
        {
            public int Id { get; set; }

            public Sender Sender { get; set; }

            public int SenderId { get; set; }
        }

        public class Sender
        {
            public int Id { get; set; }
        }
    }
}