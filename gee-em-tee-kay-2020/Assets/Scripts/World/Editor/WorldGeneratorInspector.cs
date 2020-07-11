using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorInspector : Editor
{
    List<Socks.PropertyCategory> categories = new List<Socks.PropertyCategory>();
    void OnEnable()
    {
        categories = Socks.EditorUtils.GetFields(typeof(WorldGenerator), serializedObject);
    }

    public override void OnInspectorGUI()
    {
        Socks.EditorUtils.DrawFields(serializedObject, categories);
    }
}
