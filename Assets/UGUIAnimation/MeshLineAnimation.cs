using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(UIMeshLine))]
public class MeshLineAnimation : ParentAnimation {
    UIMeshLine m_uiMeshLine = null;
    UIMeshLine owner { get { return m_uiMeshLine ?? (m_uiMeshLine = GetComponent<UIMeshLine>()); } }

    public float start = 0f;
    public float end = 1f;

    public override void Init()
    {
        owner.lengthRatio = start;
    }

    protected override void Setup() { }
    protected override void Lerp(float t)
    {
        owner.lengthRatio = Mathf.Lerp(start, end, t);
    }
}
