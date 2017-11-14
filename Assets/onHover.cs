using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class onHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public MouseManager gameMouseManager;
	// Use this for initialization
	void Start () {
		gameMouseManager = FindObjectOfType<MouseManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (eventData.pointerCurrentRaycast.gameObject != null) {
			gameMouseManager.hoveringPanel = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		gameMouseManager.hoveringPanel = false;
	}

}
