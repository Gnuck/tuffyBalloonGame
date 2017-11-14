using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void OnGUI(){
		const int buttonWidth = 84;
		const int buttonHeight = 60;

		if(
			GUI.Button(
				new Rect(
					Screen.width / 2 - (buttonWidth/2),
					(2*Screen.height / 3) - (buttonHeight / 2),
					buttonWidth,
					buttonHeight
					),
					"Start!"
					)
					)
		{
			SceneManager.LoadScene ("balloon_scene");
		}
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
