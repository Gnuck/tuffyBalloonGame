using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages state relating to the current "level"
/// </summary>
public class LevelManager : MonoBehaviour {

    public enum LevelMode { Letters, Numbers, Addition };
    public static LevelMode levelMode;

	public Text scoreText;
    public GameObject equation;
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

	public GameObject menuPanel;

	//true if a first balloon has been popped to complete the equation
	public bool firstPop;

    private void Awake()
    {
        firstEnter = true;
        Time.timeScale = 1f;
        firstPop = false;
        correctWait = true;
        firstAddend = "";
        secondAddend = "";
        secondInt = 1;
        scoreNumber = 0;
        scoreText.text = "score: " + scoreNumber;
        goalSum = Random.Range(2, 10);
        runningTotal = 0;
        updateEquation (0, false);
        recentPop = 0;

        menuPanel.SetActive(false);

        SoundManager.onEndCorrectSound += resetQuestion;
    }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(SoundManager.instance.explainAddition(goalSum));
    }
	
	// Update is called once per frame
	void Update () {
        if (!SoundManager.instance.correctWait)
        {
            correctWait = false;
        }
	}

	public void AddToScore(int points){
		scoreNumber += points;
		scoreText.text = "score: " + scoreNumber;
	}

	//Called when a balloon has been popped. Updates the addends and calls a check to see if equation is correct
	//if two balloons have recently been popped.
	public void processPop(int balloonValue){
		runningTotal += balloonValue;
        Debug.Log("runningtotal: " + runningTotal);
        Debug.Log("ballonValue: " + balloonValue);
        //If this is the first pop for the most recent problem/question
		if (runningTotal < goalSum && runningTotal == balloonValue) {
            StartCoroutine (SoundManager.instance.firstNumber(balloonValue)); 
			updateEquation (balloonValue, false);
		} 
        //if this is not the first pop and is not a correct answer (but is still not wrong yet)
        else if(runningTotal < goalSum && runningTotal != balloonValue)
        {
            StartCoroutine (SoundManager.instance.addedNumber(balloonValue));
            updateEquation(balloonValue, false);
        }
        //if the answer is wrong
		else if (runningTotal > goalSum) {
            Debug.Log("should be wrong answer");
            updateEquation(0, false);
            StartCoroutine(SoundManager.instance.WrongAnswer());
		} 
        //if the answer is correct
		else {
			updateEquation (balloonValue, true);
			StartCoroutine (SoundManager.instance.correctAnswer(balloonValue, goalSum));
        }
	}

	/* Updates the equation based on the most recent baloon pop. 
	 * param: val :  0 means  the equation is new. In that case, the equation will be updated to just
     * " = [goal number]". Also update active balloon numbers.
	 * 	-1 means the equation is completed. 
	 * 	else, the equation is updated with a new addend.
     */
	private void updateEquation(int val, bool correct){
		if (val == 0 && !correct) {
			foreach(var balloon in activeBalloons){
				string balloonNumber = (Random.Range (1,goalSum)).ToString();
				TextMesh textMeshComponent=balloon.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
				textMeshComponent.text = balloonNumber;
				balloon.GetComponent<InitialVelocity>().balloonValue = int.Parse(balloonNumber);
			}

			firstAddend = "";
			runningTotal = 0;
            equation.GetComponent<TMPro.TextMeshProUGUI>().text = " = " + goalSum;
            secondAddend = equation.GetComponent<TMPro.TextMeshProUGUI>().text;
		} 
		else if (correct) {
            equation.GetComponent<TMPro.TextMeshProUGUI>().text = firstAddend + val + secondAddend;

        }
		else {
			firstAddend += val + " + ";
            equation.GetComponent<TMPro.TextMeshProUGUI>().text = firstAddend + secondAddend; 
        }
	}

	public void menuButton(){
        menuPanel.SetActive(!menuPanel.activeInHierarchy);
        if (menuPanel.activeInHierarchy) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }

    }

    public void resumeButton(){
		menuPanel.SetActive (false);
		Time.timeScale = 1f;
	}

	public void restartButton(){
        foreach (var balloon in activeBalloons)
        {
            Destroy(balloon);
        }
        Start();
    }

	public void quitButton(){
        SoundManager.instance.QuitGame();
		SceneManager.LoadScene ("title_scene");
	}

    //Clears the current question and resets the UI to match. May explain question again based on a "coinflip"
    private void resetQuestion()
    {
        AddToScore(goalSum);
        goalSum = Random.Range(2, 10);
        firstAddend = "";
        secondAddend = "";
        updateEquation(0, false);
        //allow popped balloons to be added to the equation again.
        int coin = Random.Range(0, 3);
        if (coin == 0)
        {
            StartCoroutine(SoundManager.instance.explainAddition(goalSum));
        }
        else
        {
            correctWait = false;
        }
    }
}
