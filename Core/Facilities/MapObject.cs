using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Facilities
{
    public class MapObject<Object>
    {
        public Dictionary<string, Type> dictMapping = new Dictionary<string, Type>();

        public MapObject()
        {
            Type[] types = Assembly.GetAssembly(typeof(Object)).GetTypes(); //If (type is not derived from Object)
            foreach (Type type in types)
            {
                if (!typeof(Object).IsAssignableFrom(type) || type == typeof(Object))
                {
                    continue; //This type isn’t a Object type, keep searching through the the assembly
                }

                //Register the Object type
                dictMapping.Add(type.Name, type);
            }
        }

        public Object CreateObject(string objectName, params object[] args)
        {
            return (Object)Activator.CreateInstance(dictMapping[objectName], args);
        }

    }
}
