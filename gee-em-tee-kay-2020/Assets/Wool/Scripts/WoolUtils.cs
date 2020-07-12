using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace Wool
{
    public static class Utils
    {
        public static string FillInTags(string input)
        {
            bool isListening = false;
            string builtString = "";
            string finalString = "";
            foreach (char c in input)
            {
                if (!isListening)
                {
                    if (c == '{')
                    {
                        isListening = true;
                    }
                    else
                    {
                        finalString += c;
                    }
                }
                else if (isListening)
                {
                    if (c == '}')
                    {
                        if (builtString.Length > 0 && builtString[0] == '$')
                        {
                            // we've found a tag!
                            //finalString += Game.flags.GetFlag(builtString);
                        }
                        else
                        {
                            finalString += '{' + builtString + '}';
                        }

                        builtString = "";
                        isListening = false;
                    }
                    else
                    {
                        builtString += c;
                    }
                }
            }

            return finalString;
        }

        public static CommandCache[] GrabCommandCachesFromObject(GameObject obj)
        {
            List<CommandCache> caches = new List<CommandCache>();

            // Find every MonoBehaviour (or subclass) on the object
            foreach (var component in obj.GetComponents<MonoBehaviour>()) 
            {
                if (component == null)
                {
                    Debug.Log("COMMAND: component null: " + obj.name);
                    continue;
                }

                var type = component.GetType();

                // Find every method in this component
                foreach (var method in type.GetMethods())
                {
                    // Find the YarnCommand attributes on this method
                    var attributes = (Wool.CommandAttribute[])method.GetCustomAttributes(typeof(Wool.CommandAttribute), true);

                    // Find the YarnCommand whose commandString is equal to the command name
                    foreach (var attribute in attributes) 
                    {
                        foreach (var commandNameString in attribute.commandStrings)
                        {
                            caches.Add(new CommandCache(component, commandNameString, method, attribute.wait));
                        }
                    }
                }
            }

            return caches.ToArray();
        }

        public static Type[] GetAllDialogueEffects()
        {
            return typeof(WoolDialogueEffectBase).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(WoolDialogueEffectBase))).ToArray();
        }

        public static WoolTagToType[] GetWoolTags()
        {
            Type[] types = GetAllDialogueEffects();

            List<WoolTagToType> woolTags = new List<WoolTagToType>();
            foreach (Type type in types)
            {
                DialogueTagAttribute attr = (DialogueTagAttribute)type.GetCustomAttribute(typeof(DialogueTagAttribute));
                if (attr != null)
                {
                    foreach (string tag in attr.tag)
                    {
                        woolTags.Add(new WoolTagToType(tag, type));
                    }
                }
            }

            return (woolTags.ToArray());
        }

        public static T WithoutSelectAll<T>(Func<T> guiCall)
        {
            bool preventSelection = (Event.current.type == EventType.MouseDown);

            Color oldCursorColor = GUI.skin.settings.cursorColor;

            if (preventSelection)
                GUI.skin.settings.cursorColor = new Color(0, 0, 0, 0);

            T value = guiCall();

            if (preventSelection)
                GUI.skin.settings.cursorColor = oldCursorColor;

            return value;
        }
    }
}