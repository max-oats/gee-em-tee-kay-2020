using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AntlerGenerator))]
public class AntlerGeneratorInspector : ProcMeshGenBaseInspector
{
    List<Socks.PropertyCategory> categories = new List<Socks.PropertyCategory>();
    protected override void OnEnable()
    {
        base.OnEnable();

        categories = Socks.EditorUtils.GetFields(typeof(AntlerGenerator), serializedObject);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Socks.EditorUtils.DrawFields(serializedObject, categories, false);
    }
}
