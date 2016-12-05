using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class RayCastCheck : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler {

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<CanvasRenderer>().SetColor(Color.red);
        Debug.Log("ENTER");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<CanvasRenderer>().SetColor(Color.blue);
        Debug.Log("EXIT");
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
