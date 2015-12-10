using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(VertexLine))]
public class VertexLineEditor : Editor {

    VertexLine owner { get { return target as VertexLine; } }
    Quaternion handlesRotation;
    void OnSceneGUI()
    {
        handlesRotation = Tools.pivotRotation == PivotRotation.Local ? owner.transform.rotation : Quaternion.identity;
        for(int n =0; n < owner.Count; n++)
        {
            ShowPoint(n);
        }
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            owner.VertexUpdate();
            //undo시 호출되지 않는 문제점이 있음...
        }
    }
    void ShowPoint(int index)
    {
        EditorGUI.BeginChangeCheck();
        Vector3 point = Handles.PositionHandle(owner.transform.TransformPoint(owner[index]), handlesRotation);
        if (EditorGUI.EndChangeCheck())
        {
            //Undo.RecordObject(owner, "Move point");
            owner[index] = owner.transform.InverseTransformPoint(point);
            EditorUtility.SetDirty(owner);
        }
    }
}
