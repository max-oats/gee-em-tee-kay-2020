using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Wool.TextObject))]
public class TextObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Wool.TextObject textObject = (Wool.TextObject)target;

        if(GUILayout.Button("Update UI String"))
        {
            //todo: FIX DIS
            //textObject.UpdateUIString();
        }

        DrawDefaultInspector();
    }
}