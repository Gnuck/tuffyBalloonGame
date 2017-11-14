using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource efxSource;
	public AudioSource tuffySource;
	public static SoundManager instance = null;

	public float lowPitchRange = 0.01f;
	public float highPitchRange = 10f;



	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle (AudioClip clip){
		efxSource.clip = clip;
		efxSource.Play ();
	}

	public void RandomizeSfx (params AudioClip[] clips){
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		efxSource.pitch = randomPitch;

		efxSource.clip = clips [randomIndex];
		efxSource.Play ();
	}

	public void RandomizeTuffySfx (params AudioClip[] clips){
		int randomIndex = Random.Range (0, clips.Length);
		tuffySource.pitch = 1.0f;
		tuffySource.clip = clips [randomIndex];
		tuffySource.Play ();
	}
	 
	public void RandomizeTuffySfxEqual (params AudioClip[] clips){
		int randomIndex = Random.Range (0, clips.Length);

		tuffySource.pitch = Random.Range (0.95f, 1.1f);
		tuffySource.clip = clips [randomIndex];
		tuffySource.Play ();
	}

}
