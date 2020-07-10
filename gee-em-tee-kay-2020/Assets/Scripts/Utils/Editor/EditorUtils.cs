using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class StyleWrapper
{
    public GUIStyle style;

    public int indent = 1;

    public void Indent()
    {
        indent++;

        style.margin = new RectOffset(10, 10, 10, 10);
    }

    public void Unindent()
    {
        indent--;

        style.margin = new RectOffset(10, 10, 10, 10);
    }
    
    public static implicit operator GUIStyle(StyleWrapper sw) => sw.style;
}

namespace Socks
{
    public static class EditorUtils
    {
        public static LayerMask LayerMaskField(string label, LayerMask selected) 
        {
            List<string> layers = new List<string>();
            string[] layerNames = new string[4];
            
            int emptyLayers = 0;
            for (int i = 0; i < 32; i++) 
            {
                string layerName = LayerMask.LayerToName(i);
                
                if (layerName != "") 
                {
                    for (; emptyLayers > 0; emptyLayers--)
                    {
                        layers.Add ("Layer " + (i-emptyLayers));
                    }

                    layers.Add (layerName);
                } 
                else 
                {
                    emptyLayers++;
                }
            }
            
            if (layerNames.Length != layers.Count) 
            {
                layerNames = new string[layers.Count];
            }

            for (int i = 0; i < layerNames.Length; i++) 
            {
                layerNames[i] = layers[i];
            }
            
            selected.value = EditorGUILayout.MaskField(label, selected.value, layerNames);
            
            return selected;
        }

        public static void DrawQuad(Rect position, Color color) 
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0,0,color);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(position, GUIContent.none);
        }

        public static void ListenForChange()
        {
            EditorGUI.BeginChangeCheck();
        }

        public static bool WasChanged(Object affectedObject, string record = "Updated property")
        {
            bool wasChanged = EditorGUI.EndChangeCheck();

            if (wasChanged && affectedObject != null)
            {
                Undo.RecordObject(affectedObject, record);
            }

            return wasChanged;
        }

        public static bool WasChanged(Object affectedObject, ref bool isDirty, string record = "Updated property")
        {
            bool wasChanged = EditorGUI.EndChangeCheck();

            if (wasChanged)
            {
                isDirty = true;
                Undo.RecordObject(affectedObject, record);
            }

            return wasChanged;
        }

        public static Object FindAndLoadFirstAsset(string search, string dir, System.Type type)
        {
            string[] assets = AssetDatabase.FindAssets(search, new string[] {dir});
            if (assets.Length > 0)
            {
                return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), type);
            }

            return null;
        }

        public static Object FindAndLoadFirstAsset(string search, System.Type type)
        {
            string[] assets = AssetDatabase.FindAssets(search);
            if (assets.Length > 0)
            {
                return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), type);
            }

            return null;
        }

        public static Object[] FindAndLoadAssets(string search, string dir, System.Type type)
        {
            string[] assets = AssetDatabase.FindAssets(search, new string[] {dir});
            List<Object> objects = new List<Object>();
            foreach (string asset in assets)
            {
                if (asset.Length == 0)
                {
                    continue;
                }

                objects.Add(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), type));
            }

            return objects.ToArray();
        }

        public static (string[], Object[]) FindAndLoadAssetsWithNames(string search, string dir, System.Type type)
        {
            string[] assets = AssetDatabase.FindAssets(search, new string[] {dir});
            List<Object> objects = new List<Object>();
            List<string> names = new List<string>();
            foreach (string asset in assets)
            {
                if (asset.Length == 0)
                {
                    continue;
                }

                string name = AssetDatabase.GUIDToAssetPath(asset);
                objects.Add(AssetDatabase.LoadAssetAtPath(name, type));
                names.Add(GetAssetPathNameFromDirectory(name, dir));
            }

            return (names.ToArray(), objects.ToArray());
        }

        public static string[] GetAssetPathNames(string search, string dir)
        {
            string[] assets = AssetDatabase.FindAssets(search, new string[] {dir});
            List<Object> objects = new List<Object>();
            List<string> names = new List<string>();
            foreach (string asset in assets)
            {
                if (asset.Length == 0)
                {
                    continue;
                }

                string name = AssetDatabase.GUIDToAssetPath(asset);
                names.Add(GetAssetPathNameFromDirectory(name, dir));
            }

            return names.ToArray();
        }

        public static string GetAssetPathNameFromDirectory(string assetPath, string directory)
        {
            string built = "";
            string finalPath = assetPath.Substring(directory.Length+1);
            finalPath = finalPath.Substring(0, finalPath.LastIndexOf('.'));
            if (int.TryParse(finalPath.Substring(0, 3), out int result))
            {
                finalPath = finalPath.Substring(4);
            }
            foreach (char c in finalPath)
            {
                if (c == '/')
                {
                    built += '.';
                }
                else if (c == ' ')
                {
                    built += '_';
                }
                else
                {
                    built += c;
                }
            }

            return built;
        }

        public static List<PropertyCategory> GetFields(System.Type type, SerializedObject obj)
        {
            SerializedProperty it = obj.GetIterator();
            // MyObject myObj = ScriptableObject.CreateInstance<MyObject>();
            // SerializedObject mySerializedObject = new UnityEditor.SerializedObject(myObj);
            // SerializedProperty iterator = mySerializedObject.FindProperty("PropertyName");
            while (it.Next(true))
            {
                //Debug.Log(it.name);
            }

            List<PropertyCategory> categories = new List<PropertyCategory>();
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                FieldAttribute attribute = (FieldAttribute)field.GetCustomAttribute(typeof(FieldAttribute));
                SerializeField serializeAttribute = (SerializeField)field.GetCustomAttribute(typeof(SerializeField));
                if (attribute == null && serializeAttribute == null)
                {
                    continue;
                }

                SerializedProperty serializedProperty = obj.FindProperty(field.Name);
                if (serializedProperty == null)
                {
                    continue;
                }

                string categoryName = "Default";
                if (attribute != null && attribute.category.Length > 0)
                {
                    categoryName = attribute.category;
                }
                string propertyName = serializedProperty.displayName;
                if (attribute != null && attribute.name.Length > 0)
                {
                    propertyName = attribute.name;
                }

                PropertyCategory category = categories.Find(x => x.name == categoryName);
                if (category == null)
                {
                    category = new PropertyCategory();
                    category.name = categoryName;
                    categories.Add(category);
                }

                Property property = new Property();
                property.name = propertyName;
                property.property = serializedProperty;
                if (attribute != null)
                {
                    property.dependOn = attribute.dependOn;
                    property.readOnly = attribute.readOnly;
                }
                category.properties.Add(property);
            }

            return categories;
        }

        public static void DrawScriptName(SerializedObject obj)
        {
            EditorGUI.BeginDisabledGroup(true);
            SerializedProperty prop = obj.FindProperty("m_Script");
            EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
            EditorGUI.EndDisabledGroup();
        }

        public static bool DrawFields(SerializedObject obj, IEnumerable<PropertyCategory> categories, bool displayScriptName = true)
        {
            if (displayScriptName)
            {
                DrawScriptName(obj);
            }

            foreach (PropertyCategory category in categories)
            {
                EditorGUILayout.BeginVertical(Socks.Styles.Helpbox);
                category.show = EditorGUILayout.Foldout(category.show, category.name, true, Styles.Boldout);
                if (category.show)
                {
                    foreach (Property property in category.properties)
                    {
                        /** get whether the property is enabled */
                        bool enabled = !property.readOnly;
                        if (property.dependOn.Length > 0)
                        {
                            bool switchCase = false;
                            if (property.dependOn[0] == '!')
                            {
                                property.dependOn = property.dependOn.Substring(1);
                                switchCase = true;
                            }
                            Property findProperty = FindProperty(categories, property.dependOn);
                            if (findProperty != null)
                            {
                                if (!switchCase)
                                {
                                    enabled &= findProperty.property.boolValue;
                                }
                                else
                                {
                                    enabled &= !findProperty.property.boolValue;
                                }
                            }
                        }

                        EditorGUI.BeginDisabledGroup(!enabled);
                        {
                            DrawField(property.property, property.name);
                        }
                        EditorGUI.EndDisabledGroup();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            return obj.ApplyModifiedProperties();
        }

        public static void DrawField(SerializedProperty property, string name)
        {
            EditorGUILayout.PropertyField(property, new GUIContent(name), true);
        }

        public static Property FindProperty(IEnumerable<PropertyCategory> categories, string name)
        {
            Property property = null;
            foreach (PropertyCategory category in categories)
            {
                foreach (Property property1 in category.properties)
                {
                    if (property1.property.name == name)
                    {
                        property = property1;
                    }
                }
            }

            return property;
        }

        public static System.Type GetType( string TypeName )
        {
            // Try Type.GetType() first. This will work with types defined
            // by the Mono runtime, etc.
            var type = System.Type.GetType( TypeName );
        
            // If it worked, then we're done here
            if( type != null )
                return type;
        
            // Get the name of the assembly (Assumption is that we are using
            // fully-qualified type names)
            var assemblyName = TypeName.Substring( 0, TypeName.IndexOf( '.' ) );
        
            // Attempt to load the indicated Assembly
            var assembly = Assembly.Load( assemblyName );
            if( assembly == null )
                return null;
        
            // Ask that assembly to return the proper Type
            return assembly.GetType( TypeName );
        }
    }

    public class PropertyCategory
    {
        public string name = "";

        public List<Property> properties = new List<Property>(); 

        /** Show in editor */
        public bool show = true;
    }

    public class Property
    {
        public string name = "";

        public SerializedProperty property;

        public string dependOn = "";

        public bool readOnly = false;
    }
}