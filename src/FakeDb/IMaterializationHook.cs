namespace FakeDb
{
    public interface IMaterializationHook
    {
        void Execute(object @object); 
    }
}