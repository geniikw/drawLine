using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

//When I wrote this, only God and I understood what I was doing
//Now, God only knows

public class UIMeshLine : MaskableGraphic, IMeshModifier, ICanvasRaycastFilter
{
    [SerializeField]
    List<LinePoint> m_points = new List<LinePoint>();

    [SerializeField]
    float m_width = 10f;

    public bool eachWidth = false;

    public bool useRayCastFilter = false;

    public bool useAngle = false;

    public bool useGradient = false;
    public Gradient gradient;

    public bool fillLineJoint = false;
    public float fillDivideAngle = 25f;
    public float fillRatio = 1f;

    public float lineLength
    {
        get
        {
            float sum = 0f;
            for (int n = 0; n < m_points.Count - 1; n++)
            {
                sum += Vector2.Distance(m_points[n].point, m_points[n + 1].point);
            }
            return sum;
        }
    }

    public bool roundEdge = false;
    public int roundEdgePolygonCount = 5;
    [Range(0, 1)]
    [Header("0일땐 안그림 1일때 전부그림")]
    [SerializeField]
    float m_lengthRatio = 1f;
    public float lengthRatio
    {
        get { return m_lengthRatio; }
        set
        {
            m_lengthRatio = value;
            UpdateGeometry();
        }
    }

    /// UI Interface
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

    /// private function
    void EditMesh(VertexHelper vh)
    {
        vh.Clear();
        UIVertex[] prvVert = null;
        for (int n = 0; n < m_points.Count - 1; n++)
        {
            if (CheckLength(GetLength(n)))
            {
                break;
            }
            prvVert = DrawLine(n, vh, prvVert);

        }
    }
    UIVertex[] DrawLine(int index, VertexHelper vh, UIVertex[] prvLineVert = null)
    {
        UIVertex[] prvVert = null;
        float ratio = GetLength(index) / lineLength;
        float ratioEnd = GetLength(index + 1) / lineLength;

        float curveLength = 0f;
        float currentRatio = ratio;
        var divideCount = m_points[index].nextCurveDivideCount;

        for (int n = 0; n < divideCount; n++)
        {
            Vector3 p0 = EvaluatePoint(index, 1f / divideCount * n);
            Vector3 p1 = EvaluatePoint(index, 1f / divideCount * (n + 1));
            curveLength += Vector2.Distance(p0, p1);
        }

        for (int n = 0; n < divideCount; n++)
        {
            float t0 = 1f / divideCount * n;
            float t1 = 1f / divideCount * (n + 1);
            Vector3 p0 = EvaluatePoint(index, t0);
            Vector3 p1 = EvaluatePoint(index, t1);

            var w0 = eachWidth ? EvaluateWidth(index, t0) : m_width;
            var w1 = eachWidth ? EvaluateWidth(index, t1) : m_width;

            var a0 = useAngle ? Mathf.Lerp(m_points[index].angle, m_points[index + 1].angle, t0) : 0f;
            var a1 = useAngle ? Mathf.Lerp(m_points[index].angle, m_points[index + 1].angle, t1) : 0f;

            Color c0 = useGradient ? gradient.Evaluate(currentRatio) : color;
            float deltaRatio = Vector2.Distance(p0, p1) / curveLength * (ratioEnd - ratio);
            currentRatio += deltaRatio;
            Color c1 = useGradient ? gradient.Evaluate(currentRatio) : color;

            ///check final
            //float length = GetLength(index + 1);
            bool isFinal = false;
            if (currentRatio > m_lengthRatio)
            {
                currentRatio -= deltaRatio;
                float targetlength = lineLength * m_lengthRatio;
                Vector3 lineVector = p1 - p0;
                p1 = p0 + lineVector.normalized * (targetlength - lineLength * currentRatio);
                isFinal = true;
            }

            if (roundEdge && index == 0 && n == 0)
            {
                DrawRoundEdge(vh, p0, p1, c0, w0);
            }
            if (roundEdge && (index == m_points.Count - 2 && n == divideCount - 1 || isFinal))
            {
                DrawRoundEdge(vh, p1, p0, c1, w1);
            }

            var quad = MakeQuad(vh, p0, p1, c0, c1, a0, a1, prvVert, w0, w1);

            if (fillLineJoint && prvLineVert != null)
            {
                FillJoint(vh, quad[0], quad[1], prvLineVert, c0);
                prvLineVert = null;
            }

            if (isFinal)
                break;

            if (prvVert == null) { prvVert = new UIVertex[2]; }
            prvVert[0] = quad[3];
            prvVert[1] = quad[2];
        }
       
        return prvVert;
    }
    void FillJoint(VertexHelper vh, UIVertex vp0, UIVertex vp1, UIVertex[] prvLineVert, Color color, float width = -1)
    {
        Vector3 forwardWidthVector = vp1.position - vp0.position;
        Vector3 prvWidthVector = prvLineVert[1].position - prvLineVert[0].position;

        Vector3 prvVector = Vector3.Cross(prvWidthVector, new Vector3(0, 0, 1));

        Vector3 p0;
        Vector3 p1;
        Vector3 center = (vp0.position + vp1.position) / 2f;

        if (Vector3.Dot(prvVector, forwardWidthVector) > 0)
        {
            p0 = vp1.position;
            p1 = prvLineVert[1].position;
        }
        else
        {
            p0 = vp0.position;
            p1 = prvLineVert[0].position;
        }

        if (width < 0)
        {
            width = m_width;
        }

        Vector3 cp0 = (p0 + p1 - center * 2).normalized * width * fillRatio + center;
        float angle = Vector3.Angle(p0 - center, p1 - center);

        int currentVert = vh.currentVertCount;
        int divideCount = (int)(angle / fillDivideAngle);
        if (divideCount == 0) { divideCount = 1; }

        float unit = 1f / divideCount;

        vh.AddVert(center, color, Vector2.one * 0.5f);
        vh.AddVert(p0, color, Vector2.zero);
        for (int n = 0; n < divideCount; n++)
        {
            vh.AddVert(Curve.CalculateBezier(p0, p1, cp0, unit * (n + 1)), color, Vector2.zero);
            vh.AddTriangle(currentVert + 2 + n,currentVert, currentVert + 1 + n);
        }
    }
    /// <summary>
    /// v0          v2  
    /// ┌─────┐  ↑
    /// p0   quad   p1  width 
    /// └─────┘  ↓
    /// v1          v3
    /// 
    ///
    /// </summary>
    /// <param name="prvVert"> v0, v1 </param>
    /// <returns> {v0,v1,v2,v3}:UIVertex </returns>
    UIVertex[] MakeQuad(VertexHelper vh, Vector3 p0, Vector3 p1, Color c0, Color c1, float a0, float a1, UIVertex[] prvVert = null, float w0 = -1f, float w1 = -1f)
    {
        Vector3 lineVector = p1 - p0;
        Vector3 widthVector = Vector3.Cross(lineVector, new Vector3(0, 0, 1));
        widthVector.Normalize();

        Vector3 wV0 = useAngle ? Quaternion.Euler(0, 0, a0) * widthVector : widthVector;
        Vector3 wV1 = useAngle ? Quaternion.Euler(0, 0, a1) * widthVector : widthVector;

        UIVertex[] verts = new UIVertex[4];

        if (w0 < 0)
        {
            w0 = m_width;
        }
        if (w1 < 0)
        {
            w1 = m_width;
        }

        if (prvVert != null)
        {
            verts[0] = prvVert[0];
            verts[1] = prvVert[1];
        }
        else
        {
            verts[0].position = p0 + wV0 * w0 * 0.5f;
            verts[1].position = p0 - wV0 * w0 * 0.5f;
        }
        verts[2].position = p1 - wV1 * w1 * 0.5f;
        verts[3].position = p1 + wV1 * w1 * 0.5f;


        verts[0].uv0 = new Vector2(0, 0);
        verts[1].uv0 = new Vector2(1, 0);
        verts[2].uv0 = new Vector2(1, 1);
        verts[3].uv0 = new Vector2(0, 1);

        verts[0].color = c0;
        verts[1].color = c0;
        verts[2].color = c1;
        verts[3].color = c1;

        vh.AddUIVertexQuad(verts);
        return verts;
    }
    Vector2 EvaluatePoint(LinePoint p0, LinePoint p1, float t)
    {
        //t = t * t;//보정...
        if (p0.isNextCurve && !p1.isPrvCurve)
        {
            return Curve.CalculateBezier(p0.point, p1.point, p0.nextCurvePoint, t);
        }
        if (!p0.isNextCurve && p1.isPrvCurve)
        {
            return Curve.CalculateBezier(p0.point, p1.point, p1.prvCurvePoint, t);
        }
        if (p0.isNextCurve && p1.isPrvCurve)
        {
            return Curve.CalculateBezier(p0.point, p1.point, p0.nextCurvePoint, p1.prvCurvePoint, t);
        }
        //직선의 경우.
        return Vector2.Lerp(p0.point, p1.point, t);
    }
    Vector2 EvaluatePoint(int index, float t)
    {
        return EvaluatePoint(m_points[index], m_points[index + 1], t);
    }
    float EvaluateWidth(int index, float t)
    {
        return Mathf.Lerp(m_points[index].width, m_points[index + 1].width, t);
    }


    Vector2 GetDerivative(LinePoint p0, LinePoint p1, float t)
    {
        if (p0.isNextCurve || p1.isPrvCurve)
        {
            return Curve.CalculateBezierDerivative(p0.point, p1.point, p0.nextCurvePoint, p1.prvCurvePoint, t);
        }
        return (p1.point - p0.point).normalized;
    }
    bool CheckLength(float currentLength)
    {
        return currentLength / lineLength > m_lengthRatio;
    }
    float GetLength(int index)
    {
        if (index <= 0)
        {
            return 0f;
        }
        float sum = 0f;
        for (int n = 0; n < index; n++)
        {
            sum += Vector2.Distance(m_points[n].point, m_points[n + 1].point);
        }
        return sum;
    }

    //public function
    public LinePoint GetPointInfo(int index)
    {
        return m_points[index];
    }
    public void SetPointInfo(int index, LinePoint data)
    {
        m_points[index] = data;
        SetVerticesDirty();
    }
    public void SetPointPosition(int index, Vector2 position)
    {
        var info = m_points[index];
        info.point = position;
        m_points[index] = info;

        SetVerticesDirty();
    }

    public void AddPoint(LinePoint data)
    {
        m_points.Add(data);
        SetVerticesDirty();
    }
    public int GetPointCount()
    {
        return m_points.Count;
    }

    public Vector2 GetCurvePosition(int index, int curveIndex)
    {
        if (curveIndex >= m_points[index].nextCurveDivideCount)
        {
            throw new Exception("index Error index : " + curveIndex + " maxValue : " + m_points[index].nextCurveDivideCount);
        }

        return transform.TransformPoint(EvaluatePoint(m_points[index], m_points[index + 1], 1f / m_points[index].nextCurveDivideCount * curveIndex));
    }
    public bool IsCurve(int index)
    {
        if (m_points.Count - 1 <= index)
        {
            throw new System.Exception("인덱스가 작음 index:" + index + " maxValue : " + (m_points.Count - 1));
        }
        if (m_points[index].isNextCurve || m_points[index + 1].isPrvCurve)
            return true;

        return false;
    }

    public void DrawRoundEdge(VertexHelper vh, Vector2 p0, Vector2 p1, Color color, float width = -1)
    {
        if (width < 0)
            width = m_width;

        Vector2 widthVector = Vector3.Cross(p0 - p1, new Vector3(0, 0, 1));
        widthVector.Normalize();
        widthVector = widthVector * width / 2f;
        Vector2 lineVector = (p0 - p1).normalized * width / 2f;

        int count = roundEdgePolygonCount;
        int current = vh.currentVertCount;
        float angleUnit = Mathf.PI / (count - 1);

        vh.AddVert(p0, color, Vector2.one *0.5f);
        vh.AddVert(p0 + widthVector, color, Vector2.zero);

        for (int n = 0; n < count; n++)
        {
            vh.AddVert(p0 + Mathf.Cos(angleUnit * n) * widthVector + Mathf.Sin(angleUnit * n) * lineVector, color, Vector2.zero);
            vh.AddTriangle(current, current + 2 + n, current + 1 + n);
        }
    }

    // raycast filter interface.
    bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (!useRayCastFilter)
            return true;

        if (GetComponentInParent<Canvas>().renderMode != RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogWarning("this filter only implement at overlaymode.");
            return true;
        }


        for (int n = 0; n < GetPointCount() - 1; n++)
        {
            //Debug.Log(n + "point");
            if (CheckPointOnLine(GetPointInfo(n), GetPointInfo(n + 1), sp))
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckPointOnLine(LinePoint linePoint1, LinePoint linePoint2, Vector2 sp)
    {
        var p0 = transform.TransformPoint(linePoint1.point);
        var p1 = transform.TransformPoint(linePoint2.point);

        var c0 = transform.TransformPoint(linePoint1.nextCurvePoint);
        var c1 = transform.TransformPoint(linePoint2.prvCurvePoint);

        if (!linePoint1.isNextCurve && !linePoint2.isPrvCurve)
        {
            return CheckPointOnStraightLine(p0, p1, sp);
        }
        else
        {
            return CheckPointOnBezierCurve(p0, c0, c1, p1, sp);
        }
    }
    private bool CheckPointOnBezierCurve(Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1, Vector2 sp)
    {
        // % int
        // # double
        var t = CalculateMinT(p0, c0, c1, p1, sp);

        if (Vector3.Distance(sp, Curve.CalculateBezier(p0, p1, c0, c1, t)) < m_width / 2f)
        {
            return true;
        }

        /* unityeditor function.
        Debug.LogWarning("can't build because UnityEditor...-_-;");
        dist = UnityEditor.HandleUtility.DistancePointBezier(sp, p0, p1, c0, c1);
        */
        return false;
    }
    private float CalculateMinT(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 sp)
    {
        float mint = 0.5f;
        float currentLength = float.MaxValue;
        //float currentDot = float.MaxValue;

        for (float f = 0f; f < 1f; f += 0.02f)
        {
            var p = Curve.CalculateBezier(p0, p3, p1, p2, f);
            var l = (p.x - sp.x) * (p.x - sp.x) + (p.y - sp.y) * (p.y - sp.y);
            // Debug.Log(f + " ===> " + l);
            if (l < currentLength)
            {
                // Debug.Log("!!!!!!!!!hit!!!!!!!!");
                //var d = Curve.CalculateBezierDerivative(p0, p1, p2, p3, f);
                currentLength = l;
                mint = f;
            }
        }

        return mint;
    }

    /*http://www.tinaja.com/glib/bezdist.pdf doesn't work;
    private float CalculateMinT(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 sp) 
    {
        float z1 =float.MaxValue, s1=100f, t1=0f;
        float z2, s2, t2;
        float stepSize = 0.02f;
        var t = 0f;
        while(t < 1f)
        {
            float zbuffer;
            float sbuffer;
            Bezier(p0, p1, p2, p3, sp, t,out sbuffer,out zbuffer);
            if(sbuffer == 0)
            {
                z1 = zbuffer;
                s1 = sbuffer;
                t1 = t;
                break;
            }
            if ( t == 0f )
            {
                z1 = zbuffer;
                s1 = sbuffer;
                t1 = t;
            }
            if(sbuffer < s1)
            {
                z1 = zbuffer;
                s1 = sbuffer;
                t1 = t;
            }
            t += stepSize;
        }
        var results = 0f;
        if( s1 != 0f)
        {
            t2 = Mathf.Min(0f, t1 + stepSize);
            Bezier(p0, p1, p2, p3, sp, t2, out s2, out z2);
            if(z1 != z2)
            {
                results=(z2 * t1 - z1 * t2) / (z2 - z1);          
            }
            else
            {
                results=(t1 + t2) / 2f;
            }
        }
        else
        {
            results= t1;
        }
        return Mathf.Clamp01(results);
    }   
    private void Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 sp, float t, out float s, out float z)
    {
        var a3 = (p3.x - p0.x + 3f * (p1.x - p2.x)) / 8f;
        var b3 = (p3.y - p0.y + 3f * (p1.y - p2.y)) / 8f;
        var a2 = (p3.x + p0.x - p1.x - p2.x) * 3f / 8f;
        var b2 = (p3.y + p0.y - p1.y - p2.y) * 3f / 8f;
        var a1 = (p3.x - p0.x) / 2f - a3;
        var b1 = (p3.y - p0.y) / 2f - b3;
        var a0 = (p3.x + p0.x) / 2f - a2;
        var b0 = (p3.y + p0.y) / 2f - b2;
        var x = a0 + t * (a1 + t * (a2 + t * a3));
        var y = b0 + t * (b1 + t * (b2 + t * b3));
        var dx4 = x - sp.x;
        var dy4 = y - sp.y;
        var dx = a1 + t * (2f * a2 + t * 3f * a3);
        var dy = b1 + t * (2f * b2 + t * 3f * b3);
        z = dx * dx4 + dy * dy4;
        s = dx4 * dx4 + dy4 * dy4;
    }
    doesn't work*/

    private bool CheckPointOnStraightLine(Vector3 p0, Vector3 p1, Vector3 sp)
    {
        var v0 = p1 - p0;
        var v1 = sp - p0;

        var projectionVector = Vector3.Project(v1, v0);

        if (projectionVector.normalized != v0.normalized || projectionVector.magnitude > v0.magnitude)
            return false;

        var dist = Vector3.Distance(p0 + projectionVector, sp);

        if (dist < m_width / 2f)
            return true;

        return false;
    }

    /*debug
    void OnDrawGizmos()
    {
        DrawCubeWrap(DebugPosition1, Color.red ,Vector3.one * 3f);
        DrawCubeWrap(DebugPosition2, Color.red, Vector3.one * 3f);
        DrawCubeWrap(DebugPosition3, Color.red, Vector3.one * 3f);
        DrawCubeWrap(DebugPosition4, Color.red, Vector3.one * 3f);
    }
    private void DrawCubeWrap(Vector3 position , Color color, Vector3 size)
    {
        if (position == null)
            return;
        var buffer = Gizmos.color;
        Gizmos.color = color;
        Gizmos.DrawCube(position, size);
        Gizmos.color = buffer;
    }
    */


}