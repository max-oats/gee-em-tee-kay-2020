using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EntityManager))]
public class EntityManagerInspector : Editor
{
    List<Socks.PropertyCategory> categories = new List<Socks.PropertyCategory>();
    void OnEnable()
    {
        categories = Socks.EditorUtils.GetFields(typeof(EntityManager), serializedObject);
    }

    public override void OnInspectorGUI()
    {
        Socks.EditorUtils.DrawFields(serializedObject, categories);
    }
}
