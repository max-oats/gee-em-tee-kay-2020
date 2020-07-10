using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(InputManager))]
public class InputManagerInspector : Editor
{
    InputManager im;
    InputType[] types;

    void OnEnable()
    {
        im = target as InputManager;

        types = (InputType[])System.Enum.GetValues(typeof(InputType));
        if (im.enabledByDefault.Count != types.Length)
        {
            im.enabledByDefault = new List<bool>();
            for (int i = 0; i < types.Length; ++i)
                im.enabledByDefault.Add(false);

            EditorUtility.SetDirty(im);
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(Socks.Styles.Helpbox);
        {
            EditorGUILayout.LabelField("Enabled by default:", Socks.Styles.Bold);
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < types.Length; ++i)
            {
                im.enabledByDefault[i] = EditorGUILayout.Toggle(types[i].ToString(), im.enabledByDefault[i]);
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(im);
            }
 
        }
        EditorGUILayout.EndVertical();
    }
}