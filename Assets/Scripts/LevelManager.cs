using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public Text scoreText;
	public Text equation;
	public int scoreNumber;
	private string firstAddend;
	private string secondAddend;
	private int secondInt=1;
	public int goalSum;
	public List<GameObject> activeBalloons = new List<GameObject> ();

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

	public GameObject menuPanel;

	//true if a first balloon has been popped to complete the equation
	public bool firstPop;

	// Use this for initialization
	void Start () { 
		firstEnter = true;
		Time.timeScale = 1f;
		firstPop=false;
		correctWait = true;
		firstAddend="_";
		secondAddend="_";
		secondInt = 1;
		scoreNumber = 0;
		scoreText.text = "score: " + scoreNumber;
		goalSum = Random.Range (2, 10);
		updateEquation ();

		menuPanel.SetActive (false);

		StartCoroutine("explainGame");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddToScore(int points){
		scoreNumber += points;
		scoreText.text = "score: " + scoreNumber;
	}

	//Called when a balloon has been popped. Updates the addends and calls a check to see if equation is correct
	//if two balloons have recently been popped
	public void processPop(int balloonValue){

		//enter if first balloon already popped and second has just been popped
		if(firstPop){
			secondAddend = balloonValue.ToString();
			secondInt = balloonValue;
			SoundManager.instance.RandomizeTuffySfx (numberSounds[balloonValue-1]);
			updateEquation ();
			//check if equation is accurate and add sum to score if equation is true
			if(int.Parse(firstAddend) + int.Parse(secondAddend) == goalSum){
				StartCoroutine ("correctAnswer");
			}
			else{
				StartCoroutine ("wrongAnswer");
				firstAddend = "_";
				secondAddend = "_";
				updateEquation ();
			}

			//reset popped balloon flag
			firstPop = false;

		}
		//enter if only the first balloon for the equation has been popped
		else{
			firstAddend = balloonValue.ToString();
			SoundManager.instance.RandomizeTuffySfx (numberSounds[balloonValue-1]);
			updateEquation ();
			firstPop = true;
		}
	}

	private void updateEquation () {
		
		equation.text = (firstAddend + " + " + secondAddend + " = " + goalSum);
	}

	public void menuButton(){
		Time.timeScale = 0f;
		menuPanel.SetActive (true);

	}

	public void resumeButton(){
		menuPanel.SetActive (false);
		Time.timeScale = 1f;
	}

	public void restartButton(){
		foreach(var balloon in activeBalloons){
			Destroy (balloon);
		}
		Start ();
	}

	public void quitButton(){
		SoundManager.instance.RandomizeTuffySfx (tuffyDay,thanksForSound);
		SceneManager.LoadScene ("title_scene");
	}

	IEnumerator correctAnswer(){
		correctWait = true;
		SoundManager.instance.RandomizeTuffySfx (plusSound);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length + 0.2f);
		SoundManager.instance.RandomizeTuffySfx (numberSounds [secondInt-1]);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length + 0.2f);
		SoundManager.instance.RandomizeTuffySfxEqual (equalSound);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
		SoundManager.instance.RandomizeTuffySfx (numberSounds [goalSum - 1]);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length );
		SoundManager.instance.RandomizeTuffySfx (greatJobSound,thatsItSound,greatChoiceSound,youGotItSound);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
		AddToScore(goalSum);
		goalSum = Random.Range(2, 10);
		firstAddend = "_";
		secondAddend = "_";
		updateEquation ();
		//allow popped balloons to be added to the equation again.
		if (firstEnter) {
			StartCoroutine ("explainGame");
		} else {
			int coin = Random.Range (0, 3);
			if (coin == 0) {
				StartCoroutine ("explainGame");
			} else {
				correctWait = false;
			}
		}
	}

	IEnumerator wrongAnswer(){
		int coin = Random.Range (0, 2);
		if (coin == 0) {
			SoundManager.instance.RandomizeTuffySfx (uhOhSound, tryAgainSound);
			yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
		}
	}
		
	IEnumerator explainGame(){
		if (firstEnter) {
			SoundManager.instance.RandomizeTuffySfx (readySound);
			yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length + 0.2f);
			firstEnter = false;
		}
		SoundManager.instance.RandomizeTuffySfx (addIntro);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
		SoundManager.instance.RandomizeTuffySfx (numberSounds [goalSum - 1]);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
		correctWait = false;
	}
}
