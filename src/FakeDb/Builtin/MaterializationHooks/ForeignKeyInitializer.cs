using System;
using System.Linq;

namespace FakeDb.Builtin.MaterializationHooks
{
    public class ForeignKeyInitializer : IMaterializationHook
    {
        public void Execute(object @object)
        {
            if (@object == null) throw new ArgumentNullException("object");

            var properties = @object.GetType().GetProperties(ReflectionSettings.AllInstance);

            foreach (var property in properties)
            {
                if (!property.PropertyType.IsClass)
                    continue;

                var fkProperty = properties.SingleOrDefault(p => p.Name == property.Name + "Id");

                if (fkProperty == null)
                    continue;

                var relatedObject = property.GetValue(@object, null);

                if(relatedObject == null)
                    continue;                

                var idProperty = relatedObject.GetType().GetProperty("Id");

                if (idProperty == null)
                    continue;

                if (!fkProperty.PropertyType.IsAssignableFrom(idProperty.PropertyType))
                    continue;

                var id = idProperty.GetValue(relatedObject, null);

                fkProperty.SetValue(@object, Convert.ChangeType(id, fkProperty.PropertyType), null);
            }
        }
    }
}