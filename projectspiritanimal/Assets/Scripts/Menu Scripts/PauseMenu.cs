using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Start()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);

    }
    void Update () {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
		
	}
    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SettingsMenu()
    {
        Debug.Log("Settings button pressed");
        SceneManager.LoadScene("SettingsMenu");
    }

    public void HelpMenu()
    {
        Debug.Log("Help Icon pressed");
        SceneManager.LoadScene("HelpMenu");
    }
        
    public void QuitGame()
    {
        Debug.Log("Quit button pressed.");
     #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
     #else
        Application.Quit();
     #endif
    }
}
