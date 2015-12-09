using UnityEngine;
using System.Collections;

[System.Serializable]
public class LinePoint
{
    public Vector2 point;
    public bool isNextCurve = false;
    public Vector2 nextCurvePoint;
    public bool isPrvCurve = false;
    public Vector2 prvCurvePoint;
}

