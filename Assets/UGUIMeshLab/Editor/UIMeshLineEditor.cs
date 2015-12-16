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
        for (int n = 0; n < owner.points.Count-1; n++)
        {
            if (owner.IsCurve(n))
            {
                for (int i = 0; i < owner.divideCount-1 ; i++)
                {
                    Handles.DrawLine(owner.GetPoint(n, i), owner.GetPoint(n, i+1));
                }
            }
            else
            {
                Handles.DrawLine(owner.GetPoint(n, 0), owner.GetPoint(n + 1, 0));
            }
        }
    }
    void HandlesForPoints()
    {
        Transform ownerTrans = owner.transform;
        for (int n = 0; n < owner.points.Count; n++)
        {
            Vector2 point = owner.points[n].point;
            Vector2 cp0 = owner.points[n].nextCurvePoint;
            Vector2 cp1 = owner.points[n].prvCurvePoint;
            EditorGUI.BeginChangeCheck();
            point = Handles.PositionHandle(ownerTrans.TransformPoint(owner.points[n].point), Quaternion.identity);
            Handles.Label(point, "point" + n);
            if (owner.points[n].isNextCurve)
            {
                cp0 = Handles.PositionHandle(ownerTrans.TransformPoint(cp0), Quaternion.identity);
                Handles.Label(cp0, "curve" + n);
            }
            if (owner.points[n].isPrvCurve)
            {
                cp1 = Handles.PositionHandle(ownerTrans.TransformPoint(cp1), Quaternion.identity);
                Handles.Label(cp1, "curve" + n);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(owner, "point move");
                owner.points[n].point = ownerTrans.InverseTransformPoint(point);
                if (owner.points[n].isNextCurve)
                {
                    owner.points[n].nextCurvePoint = ownerTrans.InverseTransformPoint(cp0);
                }
                if (owner.points[n].isPrvCurve)
                {
                    owner.points[n].prvCurvePoint = ownerTrans.InverseTransformPoint(cp1);
                }
                EditorUtility.SetDirty(owner);
                owner.SetAllDirty();
            }
        }

    }
}
