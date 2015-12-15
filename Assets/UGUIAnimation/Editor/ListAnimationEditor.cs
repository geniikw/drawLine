using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ListAnimation))]
public class ListAnimationEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Button("");
    }

}
