using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour {

    public GameObject levelSelectButton;

	public void changeToScene(int sceneToChangeTo){
        levelSelectButton.SetActive(!levelSelectButton.activeInHierarchy);
		//SceneManager.LoadScene ("balloon_scene");
	}
}
