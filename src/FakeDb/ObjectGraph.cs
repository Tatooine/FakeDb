using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FakeDb
{
    public interface IObjectGraph
    {
        IEnumerable<object> Traverse(object instance);
    }

    public class ObjectGraph : IObjectGraph
    {
        static readonly HashSet<Type> IgnoreTypes = new HashSet<Type>(new[] { typeof(string) } );
 
        public IEnumerable<object> Traverse(object instance)
        {
            return TraverseCore(instance, new HashSet<object>());
        }

        static IEnumerable<object> TraverseCore(object instance, ISet<object> visited)
        {
            if (visited.Contains(instance))
                yield break;

            visited.Add(instance);

            var t = instance.GetType();

            foreach (var value in from p in t.GetProperties(ReflectionSettings.AllInstance) 
                                  where p.PropertyType.IsClass && !IgnoreTypes.Contains(p.PropertyType)
                                  select p.GetValue(instance))
            {
                if (value == null)
                    continue;

                var e = value as IEnumerable;
                if (e == null)
                {
                    foreach (var o in TraverseCore(value, visited))
                        yield return o;

                    yield return value;
                    continue;
                }

                var enumerator = e.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    foreach (var o in TraverseCore(enumerator.Current, visited))
                        yield return o;

                    yield return enumerator.Current;
                }
            }

            yield return instance;
        }
    }
}