using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorInspector : Editor
{
    WorldGenerator worldGen = null;
    List<Socks.PropertyCategory> categories = new List<Socks.PropertyCategory>();
    void OnEnable()
    {
        worldGen = target as WorldGenerator;

        categories = Socks.EditorUtils.GetFields(typeof(WorldGenerator), serializedObject);
    }

    public override void OnInspectorGUI()
    {
        Socks.EditorUtils.DrawScriptName(serializedObject);

        if (GUILayout.Button("Generate world..."))
        {
            worldGen.GenerateWorld(FindObjectOfType<EntityManager>().GetPrefab(EntityType.WorldTree));
        }

        Socks.EditorUtils.DrawFields(serializedObject, categories, false);
    }
}
