using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour {


    public delegate void OnEndCorrectSound();
    public static event OnEndCorrectSound onEndCorrectSound;

    //true if further mouseclicks need to wait for the "Correct!" sound bytes and animations to complete
    public bool correctWait;

    private bool firstEnter = true;

    //sounds for all the numbers
    public AudioClip[] numberSounds;

    //all other sounds
    public AudioClip greatJobSound;
    public AudioClip thatsItSound;
    public AudioClip hahaYeahSound;
    public AudioClip youGotItSound;
    public AudioClip uhOhSound;
    public AudioClip tryAgainSound;
    public AudioClip addIntro;
    public AudioClip numberIntro;
    public AudioClip greatChoiceSound;
    public AudioClip plusSound;
    public AudioClip equalSound;
    public AudioClip readySound;
    public AudioClip tuffyDay;
    public AudioClip thanksForSound;

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

    public void ExplainGame(int goalSum)
    {
        Debug.Log("explain game soundmanager enter");
        StartCoroutine("explainAddition", goalSum);
        Debug.Log("explain game sound manager exit");
    }

    public void QuitGame()
    {
        RandomizeTuffySfx(tuffyDay, thanksForSound);
    }

    public void RandomizeTuffySfxEqual (params AudioClip[] clips){
		int randomIndex = Random.Range (0, clips.Length);

		tuffySource.pitch = Random.Range (0.95f, 1.1f);
		tuffySource.clip = clips [randomIndex];
		tuffySource.Play ();
	}

    public IEnumerator correctAnswer(int numPop, int goalSum)
    {
        correctWait = true;
        Debug.Log("play correct answer");
        RandomizeTuffySfx(plusSound);
        yield return new WaitForSeconds(tuffySource.clip.length + 0.2f);
        RandomizeTuffySfx(numberSounds[numPop - 1]);
        yield return new WaitForSeconds(tuffySource.clip.length + 0.2f);
        RandomizeTuffySfxEqual(equalSound);
        yield return new WaitForSeconds(tuffySource.clip.length);
        RandomizeTuffySfx(numberSounds[goalSum - 1]);
        yield return new WaitForSeconds(tuffySource.clip.length);
        RandomizeTuffySfx(greatJobSound, thatsItSound, greatChoiceSound, youGotItSound);
        yield return new WaitForSeconds(tuffySource.clip.length);
        Debug.Log("done playing correct");
        correctWait = false;

        onEndCorrectSound();
    }

    public IEnumerator WrongAnswer()
    {
        int coin = Random.Range(0, 2);
        if (coin == 0)
        {
            RandomizeTuffySfx(uhOhSound, tryAgainSound);
            yield return new WaitForSeconds(tuffySource.clip.length);
        }
    }

    public IEnumerator AreYouReady()
    {
        SoundManager.instance.RandomizeTuffySfx(readySound);
        yield return new WaitForSeconds(SoundManager.instance.tuffySource.clip.length + 0.2f);
    }

    public IEnumerator AddIntro()
    {
        SoundManager.instance.RandomizeTuffySfx(addIntro);
        yield return new WaitForSeconds(SoundManager.instance.tuffySource.clip.length);
        //SoundManager.instance.RandomizeTuffySfx (numberSounds [goalSum - 1]);
        yield return new WaitForSeconds(SoundManager.instance.tuffySource.clip.length);
    }

    public IEnumerator explainAddition(int goalSum)
    {
        if (firstEnter)
        {
            RandomizeTuffySfx(readySound);
            yield return new WaitForSeconds(tuffySource.clip.length + 0.2f);
            firstEnter = false;
        }
        SoundManager.instance.RandomizeTuffySfx(addIntro);
        yield return new WaitForSeconds(SoundManager.instance.tuffySource.clip.length);
        SoundManager.instance.RandomizeTuffySfx (numberSounds [goalSum - 1]);
        yield return new WaitForSeconds(SoundManager.instance.tuffySource.clip.length);
        correctWait = false;
    }

    public IEnumerator firstNumber(int popNum)
    {
        Debug.Log("first number sound enter");
        RandomizeTuffySfx (numberSounds [popNum - 1]);
        yield return new WaitForSeconds(tuffySource.clip.length + 0.2f);
        Debug.Log("first number sound exit");
    }

    public IEnumerator addedNumber(int popNum)
    {
        Debug.Log("added number sound enter");
        RandomizeTuffySfx(plusSound);
        yield return new WaitForSeconds(tuffySource.clip.length);
        RandomizeTuffySfx (numberSounds [popNum-1]);
        yield return new WaitForSeconds(tuffySource.clip.length + 0.2f);
        Debug.Log("added number sound exit");
    }

    //IEnumerator addedNumber()
    //{
    //    if (runningTotal == recentPop)
    //    {
    //        //SoundManager.instance.RandomizeTuffySfx (numberSounds [recentPop - 1]);
    //        yield return new WaitForSeconds(SoundManager.instance.tuffySource.clip.length + 0.2f);
    //    }
    //    else
    //    {
    //        SoundManager.instance.RandomizeTuffySfx(plusSound);
    //        yield return new WaitForSeconds(SoundManager.instance.tuffySource.clip.length);
    //        //SoundManager.instance.RandomizeTuffySfx (numberSounds [recentPop-1]);
    //        yield return new WaitForSeconds(SoundManager.instance.tuffySource.clip.length + 0.2f);
    //    }
    //}
}
