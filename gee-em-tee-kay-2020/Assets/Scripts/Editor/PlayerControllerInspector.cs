using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerInspector : Editor
{
    List<Socks.PropertyCategory> categories = new List<Socks.PropertyCategory>();
    void OnEnable()
    {
        categories = Socks.EditorUtils.GetFields(typeof(PlayerController), serializedObject);
    }

    public override void OnInspectorGUI()
    {
        Socks.EditorUtils.DrawFields(serializedObject, categories);
    }
}
