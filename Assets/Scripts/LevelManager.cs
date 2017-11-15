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
	private int runningTotal;
	private int recentPop;

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
		firstAddend="";
		secondAddend="";
		secondInt = 1;
		scoreNumber = 0;
		scoreText.text = "score: " + scoreNumber;
		goalSum = Random.Range (2, 10);
		runningTotal = 0;
		//updateEquation ();
		updateEquation2(0);
		recentPop = 0;

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
	//if two balloons have recently been popped.
	public void processPop(int balloonValue){
		recentPop = balloonValue;
		runningTotal += balloonValue;
		if (runningTotal < goalSum) {
			StartCoroutine ("addedNumber");
			updateEquation2 (balloonValue);
		} 
		else if (runningTotal > goalSum) {
			StartCoroutine ("wrongAnswer");
		} 
		else {
			updateEquation2 (-1);
			StartCoroutine ("correctAnswer");
		}
	}

	//updates the equation based on the most recent pop. 
	/* param: val :  0 means  the equation is new. In that case, the equation will be updated to just " = [goal number]". Also update active balloon numbers.
	 * 			 	-1 means the equation is completed. 
	 * 				else, the equation is updated with a new addend.*/
	private void updateEquation2(int val){
		if (val == 0) {

			foreach(var balloon in activeBalloons){
				string balloonNumber = (Random.Range (1,goalSum)).ToString();
				TextMesh textMeshComponent=balloon.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
				textMeshComponent.text = balloonNumber;
				balloon.GetComponent<InitialVelocity>().balloonValue = int.Parse(balloonNumber);
			}

			firstAddend = "";
			runningTotal = 0;
			equation.text = " = " + goalSum;
			secondAddend = equation.text;
		} 
		else if (val == -1) {
			equation.text = firstAddend + recentPop + secondAddend;
		}
		else {
			firstAddend += val + " + ";
			equation.text = firstAddend + secondAddend;
		}
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
		SoundManager.instance.RandomizeTuffySfx (numberSounds [recentPop-1]);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length + 0.2f);
		SoundManager.instance.RandomizeTuffySfxEqual (equalSound);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
		SoundManager.instance.RandomizeTuffySfx (numberSounds [goalSum - 1]);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length );
		SoundManager.instance.RandomizeTuffySfx (greatJobSound,thatsItSound,greatChoiceSound,youGotItSound);
		yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
		AddToScore(goalSum);
		goalSum = Random.Range(2, 10);
		firstAddend = "";
		secondAddend = "";
		//updateEquation ();
		updateEquation2(0);
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
		updateEquation2(0);
		int coin = Random.Range (0, 2);
		if (coin == 0) {
			SoundManager.instance.RandomizeTuffySfx (uhOhSound, tryAgainSound);
			yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
		}
	}

	IEnumerator addedNumber(){
		if (runningTotal == recentPop) {
			SoundManager.instance.RandomizeTuffySfx (numberSounds [recentPop - 1]);
			yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length + 0.2f);
		}
		else {
			SoundManager.instance.RandomizeTuffySfx (plusSound);
			yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length);
			SoundManager.instance.RandomizeTuffySfx (numberSounds [recentPop-1]);
			yield return new WaitForSeconds (SoundManager.instance.tuffySource.clip.length + 0.2f);
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
