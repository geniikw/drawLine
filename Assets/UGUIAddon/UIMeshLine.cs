using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;

public class UIMeshLine : MaskableGraphic, IMeshModifier
{
    public AnimationCurve curve;
    public List<LinePoint> list = new List<LinePoint>();

    public void ModifyMesh(VertexHelper vh)
    {
        EditMesh(vh);
    }

    public void ModifyMesh(Mesh mesh)
    {
        using (var vh = new VertexHelper(mesh))
        {
            EditMesh(vh);
            vh.FillMesh(mesh);
        }
    }

    void EditMesh(VertexHelper vh)
    {
        vh.Clear();
        for(int n  = 0; n < list.Count-1; n++)
        {


        }
    }


    /// <summary>
    /// t가 0에서 1사이일때 p0 와 p1 사이를 보간..?
    /// </summary>
    Vector2 EvaluatePoint(LinePoint p0, LinePoint p1, float t)
    {
        if(p0.isNextCurve || p1.isPrvCurve)
        {
            float oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * oneMinusT * p0.point +
                    3f * oneMinusT * oneMinusT * t * p0.nextCurvePoint +
                    3f * oneMinusT * t * t * p1.prvCurvePoint +
                    t * t * t * p1.point;
        }
        //직선의 경우.
        return Vector2.Lerp(p0.point, p1.point, t);
    }
    /// <summary>
    /// 의미가 있을지 모르겠지만 기울기임 ㅋ
    /// </summary>

    Vector2 GetDerivative(LinePoint p0, LinePoint p1, float t)
    {
        if(p0.isNextCurve || p1.isPrvCurve)
        {
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p0.nextCurvePoint - p0.point) +
                6f * oneMinusT * t * (p1.prvCurvePoint - p0.nextCurvePoint) +
                3f * t * t * (p1.point - p1.prvCurvePoint);
        }
        return (p1.point - p0.point).normalized;
    }


}
