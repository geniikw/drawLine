using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UIMeshLine))]
public class UIMeshLineEditor : Editor {
    
    UIMeshLine m_UIMeshLine;
    UIMeshLine owner { get { return m_UIMeshLine ?? (m_UIMeshLine = target as UIMeshLine);} }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    void OnSceneGUI()
    {
        HandlesForPoints();
        //DrawLineInScene(); //for debug
    }
    void DrawLineInScene()
    {
        Handles.color = Color.white;
        for (int n = 0; n < owner.GetPointCount()-1; n++)
        {
            if (owner.IsCurve(n))
            {
                for (int i = 0; i < owner.GetPointInfo(n).nextCurveDivideCount-1 ; i++)
                {
                    Handles.DrawLine(owner.GetCurvePosition(n, i), owner.GetCurvePosition(n, i+1));
                }
            }
            else
            {
                Handles.DrawLine(owner.GetCurvePosition(n, 0), owner.GetCurvePosition(n + 1, 0));
            }
        }
    }
    void HandlesForPoints()
    {
        Transform ownerTrans = owner.transform;
        for (int n = 0; n < owner.GetPointCount(); n++)
        {
            Vector3 point = owner.GetPointInfo(n).point;
            Vector3 cp0 = owner.GetPointInfo(n).nextCurvePoint;
            Vector3 cp1 = owner.GetPointInfo(n).prvCurvePoint;

            EditorGUI.BeginChangeCheck();
            point = Handles.PositionHandle(ownerTrans.TransformPoint(owner.GetPointInfo(n).point), Quaternion.identity);
            Handles.Label(point, "point" + n);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.FindProperty("m_points").GetArrayElementAtIndex(n).FindPropertyRelative("point").vector2Value = ownerTrans.InverseTransformPoint(point);
                serializedObject.ApplyModifiedProperties();
            }
            point = owner.transform.InverseTransformPoint(point);

            if (owner.GetPointInfo(n).isNextCurve)
            {
                EditorGUI.BeginChangeCheck();
                cp0 = Handles.PositionHandle(ownerTrans.TransformPoint(cp0), Quaternion.identity);
                Handles.Label(cp0, "curve" + n);

                if (EditorGUI.EndChangeCheck())
                {
                    cp0 = owner.transform.InverseTransformPoint(cp0);
                    serializedObject.FindProperty("m_points").GetArrayElementAtIndex(n).FindPropertyRelative("nextCurveOffset").vector2Value
                        = cp0 - point;
                    serializedObject.ApplyModifiedProperties();
                }
            }
            

            if (owner.GetPointInfo(n).isPrvCurve)
            {
                EditorGUI.BeginChangeCheck();
                cp1 = Handles.PositionHandle(ownerTrans.TransformPoint(cp1), Quaternion.identity);
                Handles.Label(cp1, "curve" + n);
                if (EditorGUI.EndChangeCheck())
                {
                    cp1 = owner.transform.InverseTransformPoint(cp1);
                    serializedObject.FindProperty("m_points").GetArrayElementAtIndex(n).FindPropertyRelative("prvCurveOffset").vector2Value
                        = cp1 - point;
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

    }
}


[CustomEditor(typeof(UIPolygon))]
public class UIPolygonEditor : Editor
{
    UIPolygonEditor m_UIMeshLine;
    UIPolygonEditor owner { get { return m_UIMeshLine ?? (m_UIMeshLine = target as UIPolygonEditor); } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
