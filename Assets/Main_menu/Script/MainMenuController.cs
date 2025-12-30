using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject howToPlayPanel;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }


    public void StartGame()
    {
      

      //  SceneManager.LoadScene("GameScene"); // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenHowToPlay()
    {
        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
