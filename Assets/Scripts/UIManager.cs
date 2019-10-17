using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel;
    public LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void menuButton()
    {
        Time.timeScale = 0f;
        menuPanel.SetActive(true);

    }

    public void resumeButton()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void restartButton()
    {
        levelManager.restartButton();
    }

    public void quitButton()
    {
        SoundManager.instance.QuitGame();
        SceneManager.LoadScene("title_scene");
    }

    public void startLetters()
    {
        LevelManager.levelMode = LevelManager.LevelMode.Letters;
        SceneManager.LoadScene("balloon_scene");
    }

    public void startNumbers()
    {
        LevelManager.levelMode = LevelManager.LevelMode.Numbers;
        SceneManager.LoadScene("balloon_scene");
    }

    public void startAddition()
    {
        LevelManager.levelMode = LevelManager.LevelMode.Addition;
        SceneManager.LoadScene("balloon_scene");
    }
}
