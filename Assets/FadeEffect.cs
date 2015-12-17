using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour {
    RectTransform m_rect;
    RectTransform rect { get { return m_rect ?? (m_rect = GetComponent<RectTransform>()); } }
    
	// Use this for initialization
	void Start () {
        
	}
	
}
