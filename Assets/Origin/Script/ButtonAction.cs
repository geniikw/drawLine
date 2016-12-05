using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ButtonAction : MonoBehaviour , IPointerClickHandler{

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked") ;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
