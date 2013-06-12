namespace FakeDb.Tests
{
    public interface IFixture<out T> 
    {
        T Create();
    }
}