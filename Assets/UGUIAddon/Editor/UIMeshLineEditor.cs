using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UIMeshLine))]
public class UIMeshLineEditor : Editor {
    
    UIMeshLine m_UIMeshLine;
    UIMeshLine owner { get { return m_UIMeshLine ?? (m_UIMeshLine = target as UIMeshLine);} }
 
    void OnSceneGUI()
    {
        HandlesForPoints();
        DrawLineInScene();
    }
    void DrawLineInScene()
    {
        Handles.color = Color.white;
        Transform ownerTrans = owner.transform;

        for (int n = 0; n < owner.points.Count - 1; n++)
        {
            if (owner.IsCurve(n))
            {
                float divide = owner.divideCount;

                for (float i = 0; i < divide; i++)
                {
                    Handles.DrawLine(ownerTrans.TransformPoint(owner.EvaluatePoint(n, 1f / divide * i)), ownerTrans.TransformPoint(owner.EvaluatePoint(n, 1f / divide * (i + 1f))));
                }
            }
            else
            {
                Handles.DrawLine(ownerTrans.TransformPoint(owner.points[n].point), ownerTrans.TransformPoint(owner.points[n + 1].point));
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
