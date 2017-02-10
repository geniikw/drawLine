using UnityEngine;
using System.Collections;

/// <summary>
/// 주의 : inGame 모듈에선 isFold를 쓰면 안됨 쓸일도 없겠지만...
/// </summary>
[System.Serializable]
public struct LinePoint
{
    public Vector2 point;
    public bool isNextCurve;
    public Vector2 nextCurveOffset;
    public bool isPrvCurve;
    public Vector2 prvCurveOffset;

    public Vector2 nextCurvePoint
    {
        get
        { return nextCurveOffset + point; }
        set { nextCurveOffset = value - point; }
    }
    public Vector2 prvCurvePoint { get { return prvCurveOffset + point; } set { prvCurveOffset = value - point; } }
    [Range(1, 100)]
    public int nextCurveDivideCount;
    [Range(0, 200)]
    public float width;

    public float angle;

    public LinePoint(Vector3 p)
    {
        point = p;
        isNextCurve = false;
        isPrvCurve = false;
        nextCurveOffset = Vector3.zero;
        prvCurveOffset = Vector3.zero;
        nextCurveDivideCount = 10;
        width = 10f;
        angle = 0f;

#if UNITY_EDITOR
        isFold = false;
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// 이 값은 에디팅에만 필요하고 게임에는 필요 없지만 방법이 없어서 그냥 넣어둠...
    /// </summary>
    public bool isFold;
#endif
}