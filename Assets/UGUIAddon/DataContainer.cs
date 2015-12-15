using UnityEngine;
using System.Collections;

/// <summary>
/// 주의 : inGame 모듈에선 isFold를 쓰면 안됨 쓸일도 없겠지만...
/// </summary>
[System.Serializable]
public class LinePoint
{
    public Vector2 point;
    public bool isNextCurve = false;
    public Vector2 nextCurveOffset;
    public bool isPrvCurve = false;
    public Vector2 prvCurveOffset;

    //todo(solved) : CurvePoint가 Offset값으로 넣엇는데 지금은 좌표로 적용되어 있음.
    public Vector2 nextCurvePoint { get { return nextCurveOffset + point; } set{ nextCurveOffset = value - point; } }
    public Vector2 prvCurvePoint {  get { return prvCurveOffset + point; } set { prvCurveOffset = value - point; } }
    
#if UNITY_EDITOR
    /// <summary>
    /// 이 값은 에디팅에만 필요하고 게임에는 필요 없지만 방법이 없어서 그냥 넣어둠...
    /// </summary>
    public bool isFold = false;
#endif
}

