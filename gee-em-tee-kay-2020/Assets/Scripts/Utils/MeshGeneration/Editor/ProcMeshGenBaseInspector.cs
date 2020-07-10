using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ProcMeshGenBase))]
public class ProcMeshGenBaseInspector : Editor
{
    ProcMeshGenBase pmgb;
    List<Socks.PropertyCategory> categories = new List<Socks.PropertyCategory>();
    protected virtual void OnEnable()
    {
        pmgb = target as ProcMeshGenBase;
        categories = Socks.EditorUtils.GetFields(typeof(ProcMeshGenBase), serializedObject);

        pmgb.Init();

        if (pmgb.meshFilter.sharedMesh == null)
        {
            pmgb.BeginMeshGen();
        }
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate mesh"))
        {
            pmgb.BeginMeshGen();
        }
    }
}
