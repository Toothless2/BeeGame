using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BeeGame.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Allows the copying of a class by value useing reflection
        /// </summary>
        /// <param name="obj">Object to copy</param>
        /// <returns>a new object with all values copyed</returns>
        /// <remarks>
        /// Mush faster than the serialize method however alot more complicated
        /// </remarks>
        public static T CloneObject<T>(this T obj)
        {
            //gets the tyoe of the given object
            Type typeSource = obj.GetType();

            //makes a new object of type T
            T objTarget = (T)Activator.CreateInstance(typeSource);

            //gets the properties in T
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //applies the properties in T to the new type T object
            foreach (var property in propertyInfo)
            {
                if (property.CanWrite)
                {
                    //if the propertly is a value just set it
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(string)))
                    {
                        property.SetValue(objTarget, property.GetValue(obj, null), null);
                    }
                    else
                    {
                        //if the propertly is not a value type this function will need to be called recursivly as it could also have non value type veriables
                        object propertyValue = property.GetValue(obj, null);

                        if (propertyValue == null)
                        {
                            property.SetValue(obj, null, null);
                        }
                        else
                        {
                            property.SetValue(obj, propertyValue.CloneObject(), null);
                        }
                    }
                }

            }
            return objTarget;
        }
    }
}
