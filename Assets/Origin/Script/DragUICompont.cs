using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(UIMeshLine))]
public class DragUICompont : MonoBehaviour, IDragHandler
{
    UIMeshLine m_line;
    void Start()
    {
        m_line = GetComponent<UIMeshLine>();
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        m_line.SetPointPosition(0, eventData.position);
    }
}
