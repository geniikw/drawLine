using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CanvasRenderer))]
public class ColorAnimation : ParentAnimation {

    CanvasRenderer m_cr;
    CanvasRenderer canvasRenderer { get { return m_cr ?? (m_cr = GetComponent<CanvasRenderer>()); } }
    public Color target;
    

    protected override IEnumerator PlayAnimation()
    {
        Color startbuffer = canvasRenderer.GetColor();
        float t = 0f;
        while(t < 1f)
        {
            canvasRenderer.SetColor(Color.Lerp(startbuffer, target, t));
                        
            t += Time.deltaTime/time;
            yield return null;
        }


    }
}
