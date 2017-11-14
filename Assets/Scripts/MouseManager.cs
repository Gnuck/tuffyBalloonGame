using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using NUnit.Framework;
using System;

public class MouseManager : MonoBehaviour {

	public LevelManager gameLevelManager;

	public bool hoveringPanel = false;

	void Start() {
		gameLevelManager = FindObjectOfType<LevelManager> ();
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){

			Vector3 mouseWorldPos3D = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 mousePos2D = new Vector2 (mouseWorldPos3D.x,mouseWorldPos3D.y);
			Vector2 dir = Vector2.zero;


			RaycastHit2D hit = Physics2D.Raycast (mousePos2D, dir);

			if(hit!=null && hit.collider!=null && !hoveringPanel){
				// clicked on a collider

				if(hit.collider.attachedRigidbody != null){
					//don't do anything if the balloon has already been popped. Don't pop twice
					if (hit.collider.GetComponent<Animator> ().GetBool ("popped")){
						//do nothing
					}
					else{
						//skip processing the pop for equation purposes if the "Correct!" animation is still active
						if(!gameLevelManager.correctWait){
							gameLevelManager.processPop (hit.collider.GetComponent<InitialVelocity> ().balloonValue);
						}
						hit.collider.GetComponent<Animator>().SetBool("popped",true);
					}
				}
			}
		}
	}

	void FixedUpdate(){
		
	}
}
