using UnityEngine;
using UnityEngine.SceneManagement;

//Navigation in main menu
public class MainMenu : MonoBehaviour
{
    public GameObject HelpMenu;
    public GameObject SettingsMenu;

    public UIGrid MenuGrid;
    public GameObject StartButton;
    public GameObject StartLANButton;
    public GameObject HelpButton;
    public GameObject SettingsButton;
    public GameObject ExitButton;

    

    public void OnStartGame()
    {
        SceneManager.LoadScene(1);
    }

   
    public void OnExit()
    {
        Application.Quit();
    }

    public void OnHelp()
    {
        HelpMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnSettings()
    {
        SettingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}