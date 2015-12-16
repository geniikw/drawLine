using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(UIMeshLine))]
public class MeshLineAnimation : ParentAnimation {
    UIMeshLine m_uiMeshLine = null;
    UIMeshLine owner { get { return m_uiMeshLine ?? (m_uiMeshLine = GetComponent<UIMeshLine>()); } }

    float start = 0f;
    float end = 1f;

    public override void Init()
    {
        owner.lengthRatio = start;
    }

    protected override IEnumerator PlayAnimation()
    {
        float t = 0;
        while(t < 1f)
        {
            owner.lengthRatio = Mathf.Lerp(start, end, t);
           
            t += Time.deltaTime/time;
            yield return null;
        }
    }
}
