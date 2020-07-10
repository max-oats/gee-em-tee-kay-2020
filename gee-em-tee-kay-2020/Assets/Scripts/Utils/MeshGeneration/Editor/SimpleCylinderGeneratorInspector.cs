using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SimpleCylinderGenerator))]
public class SimpleCylinderGeneratorInspector : ProcMeshGenBaseInspector
{
    List<Socks.PropertyCategory> categories = new List<Socks.PropertyCategory>();
    protected override void OnEnable()
    {
        base.OnEnable();
        categories = Socks.EditorUtils.GetFields(typeof(SimpleCylinderGenerator), serializedObject);
    }

    public override void OnInspectorGUI()
    {
        Socks.EditorUtils.DrawScriptName(serializedObject);
        base.OnInspectorGUI();
        Socks.EditorUtils.DrawFields(serializedObject, categories, false);
    }
}
