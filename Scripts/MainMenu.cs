using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject levelsPanel;
    public GameObject keybindsPanel;

    public void PlayGame()
    {
        // Show the levels panel
        levelsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void OpenKeybinds()
    {
        // Show the keybinds panel
        keybindsPanel.SetActive(true);
    }
}