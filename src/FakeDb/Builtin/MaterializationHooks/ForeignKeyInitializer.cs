using System;
using System.Linq;

namespace FakeDb.Builtin.MaterializationHooks
{
    public class ForeignKeyInitializer : IMaterializationHook
    {
        readonly IIdPropertyFinder _idPropertyFinder;
        readonly IForeignKeyFinder _foreignKeyFinder;

        public ForeignKeyInitializer(IIdPropertyFinder idPropertyFinder = null, IForeignKeyFinder foreignKeyFinder = null)
        {
            _idPropertyFinder = idPropertyFinder ?? new IdPropertyFinder();
            _foreignKeyFinder = foreignKeyFinder ?? new ForeignKeyFinderFinder();
        }

        public void Execute(object @object)
        {
            if (@object == null) throw new ArgumentNullException("object");

            var properties = @object.GetType().GetProperties(ReflectionSettings.AllInstance);

            foreach (var property in properties)
            {
                if (!property.PropertyType.IsClass)
                    continue;

                var fkProperty = _foreignKeyFinder.Find(property);

                if (fkProperty == null)
                    continue;

                var relatedObject = property.GetValue(@object, null);

                if(relatedObject == null)
                    continue;

                var idProperty = _idPropertyFinder.Find(property.PropertyType);

                if (idProperty == null)
                    continue;

                if (!fkProperty.PropertyType.IsAssignableFrom(idProperty.PropertyType))
                    continue;

                var id = idProperty.GetValue(relatedObject, null);

                fkProperty.SetValue(@object, id, null);
            }
        }
    }
}