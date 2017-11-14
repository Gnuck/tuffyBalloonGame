using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour {
	public float delay = 0f;
	Animator anim;
	public AudioClip popSound;
	LevelManager levelManager; 

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		levelManager= FindObjectOfType<LevelManager> ();
	}

	void popBalloon(){
		SoundManager.instance.RandomizeSfx (popSound);

		levelManager.activeBalloons.Remove (gameObject);
		Destroy (gameObject, anim.GetCurrentAnimatorStateInfo(0).length + delay); 
	}
}
