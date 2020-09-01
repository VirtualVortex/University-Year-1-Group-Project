using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour {

    public GameObject mainMenuObject;

    [Header("For quit animation")]
    [SerializeField]
    private bool forAnimation;
    public Animator ani;
    [SerializeField]
    private Image sr;
    [SerializeField]
    private Sprite sprite;
    private bool quitting;

    private void Start()
    {
        mainMenuObject.SetActive(true);
        quitting = false;
    }

    public void ToggleMenuPanels()
    {
        mainMenuObject.SetActive(!mainMenuObject.activeSelf);
    }

    public void StartButton()
    {
        Debug.Log("Start button pressed.");
        SceneManager.LoadScene("Level1");
    }
    
    public void HelpButton()
    {
        Debug.Log("Help button pressed.");
        SceneManager.LoadScene("Help-Menu");
    }

    public void SettingsButton()
    {
        Debug.Log("Settings button pressed.");
        SceneManager.LoadScene("SettingsMenu");
    }
    
    public void PressedQuitButton()
    {
        //Debug.Log("Quit button pressed.");

        if (forAnimation)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            sr.sprite = sprite;
            quitting = true;
            ani.SetBool("Quitting", quitting);
            Debug.Log("Quit button pressed.");
        }
/*        
#if UNITY_EDITOR
            
#else
        sr.sprite = sprite;
        ani.Play("AroDeathSceneAnimation",0,0);
#endif
*/        

    }

    public void QuitGame()
    {
        Debug.Log("Quitting. If you are in the editor, nothing will happen!");
        Application.Quit();
    }

    public void MenuButton()// I added this function so that I could use the same script on my game over menu and call the Main Menu
    {
        Debug.Log("Menu button pressed");
        SceneManager.LoadScene("MainMenu");
    }

    public void ControlsMenu()
    {
        Debug.Log("Controls button pressed");
        SceneManager.LoadScene("ControlsMenu");
    }
}
