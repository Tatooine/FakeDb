using System.Reflection;

namespace FakeDb
{
    public class ReflectionSettings
    {
        public const BindingFlags AllInstance = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
    }
}