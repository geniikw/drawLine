using UnityEngine;
using System.Collections;



public static class Curve
{
    public static Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 cp0, Vector3 cp1, float t)
    {
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * cp0 +
                3f * oneMinusT * t * t * cp1 +
                t * t * t * p1;
    }
    public static Vector2 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 cp0, float t)
    {
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * p0 +
               2f * oneMinusT * t * cp0 +
               t * t * p1;
    }
}

