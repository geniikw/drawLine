using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CanvasRenderer))]
public class ColorAnimation : ParentAnimation
{

    CanvasRenderer m_cr;
    CanvasRenderer canvasRenderer { get { return m_cr ?? (m_cr = GetComponent<CanvasRenderer>()); } }
    public Color target;
    Color start;



    protected override void Setup()
    {
        start = canvasRenderer.GetColor();
    }
    protected override void Lerp(float t)
    {
        canvasRenderer.SetColor(Color.Lerp(start, target, t));
    }
}

