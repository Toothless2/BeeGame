using System;
using System.Reflection;
using UnityEngine;
using BeeGame.Items;

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
            //* gets the tyoe of the given object
            Type typeSource = obj.GetType();

            //* makes a new object of type T
            T objTarget = (T)Activator.CreateInstance(typeSource);

            //* gets the properties in T
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //* applies the properties in T to the new type T object
            foreach (var property in propertyInfo)
            {
                if (property.CanWrite)
                {
                    //* if the propertly is a value just set it
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(string)))
                    {
                        property.SetValue(objTarget, property.GetValue(obj, null), null);
                    }
                    else
                    {
                        //* if the propertly is not a value type this function will need to be called recursivly as it could also have non value type veriables
                        object propertyValue = property.GetValue(obj, null);

                        if (propertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, propertyValue.CloneObject(), null);
                        }
                    }
                }
            }

            //* gets all of the field in T
            FieldInfo[] fieldInfo = typeSource.GetFields();

            //* applies all of the fiels of T to the new object if type T in the same manor that the properites are applied
            foreach (var field in fieldInfo)
            {
                if(field.FieldType.IsValueType || field.FieldType.IsEnum || field.FieldType.Equals(typeof(string)))
                {
                    field.SetValue(objTarget, field.GetValue(obj));
                }
                else
                {
                    object fieldValue = field.GetValue(obj);

                    if(fieldValue == null)
                    {
                        field.SetValue(objTarget, null);
                    }
                    else
                    {
                        field.SetValue(objTarget, field.CloneObject());
                    }
                }
            }

            return objTarget;
        }
        
        /// <summary>
        /// Will colour the sprite given a colour and optionaly colours to avoid
        /// </summary>
        /// <param name="sprite">Sprite to colour</param>
        /// <param name="colour">Colour to set the sprite to</param>
        /// <param name="coloursToAvoid">Colours to avoid, Optional</param>
        /// <param name="setTransparentToWhite">Should transparent value to set wo white, Default <see cref="true"/></param>
        /// <returns></returns>
        public static Sprite ColourSprite(this Sprite sprite, Color colour, Color[] coloursToAvoid = null, bool setTransparentToWhite = false)
        {
            Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            //* sets the teture pixels to the pixels of teh sprite so the original sprite is not modified
            tex.SetPixels(sprite.texture.GetPixels());

            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    //* if we dont have to avoid any colours set the pixel
                    if (coloursToAvoid == null)
                    {
                        tex.SetPixel(x, y, tex.GetPixel(x, y) * colour);
                    }
                    else
                    {
                        for (int i = 0; i < coloursToAvoid.Length; i++)
                        {
                            //* if this colour should be avoided skip this iteration of the loop and move on
                            if (tex.GetPixel(x, y) == coloursToAvoid[i])
                                goto Skip;
                        }

                        tex.SetPixel(x, y, tex.GetPixel(x, y) * colour);
                    }

                    //* if transparent pixels should be set to white do that
                    if (setTransparentToWhite && tex.GetPixel(x, y).a == 0)
                        tex.SetPixel(x, y, Color.white);

                    Skip:
                        continue;
                }
            }

            //* apply the new texture with its colours
            tex.Apply();
            
            //* return the Texture2D as a sprite
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new THVector2(0.5f, 0.5f));
        }
        
        public static void SpawnItem(this Item item, THVector3 position, Quaternion rotation = new Quaternion())
        {
            GameObject go = MonoBehaviour.Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, position, rotation) as GameObject;
            go.GetComponent<ItemGameObject>().item = item;
        }
    }
}
