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
}
