using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelsPanel;
    public GameObject keybindsPanel;

    public void PlayGame()
    {
        // Show the levels panel and hide the main menu panel
        levelsPanel.SetActive(true);
        //mainMenuPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void OpenKeybinds()
    {
        // Show the keybinds panel and hide the main menu panel
        keybindsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
}