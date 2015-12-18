using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class Screen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("down");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("up");
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
