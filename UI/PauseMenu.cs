using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private int selectedSceneIndex = 0;
    private string[] sceneNames;
    private bool isPaused = false;
    public bool isChangingLevel = false;
    private bool isShowingKeybinds = false;

    public Texture2D[] levelImages;


    private void Start()
    {
        // Get all scene names
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        sceneNames = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }
    }

    private void OnGUI()
    {
        if (isPaused)
        {
            // Create a GUIStyle with a semi-transparent dark background
            GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
            backgroundStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 0.8f));

            // Draw the background
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", backgroundStyle);

            // Calculate the center of the screen
            float centerX = Screen.width / 2;
            float centerY = Screen.height / 2;

            // Create a GUIStyle for the text with white lettering
            GUIStyle textStyle = new GUIStyle(GUI.skin.label);
            textStyle.normal.textColor = Color.white;
            textStyle.alignment = TextAnchor.MiddleCenter;

            // Display "Game Paused" text
            GUI.Label(new Rect(centerX - 100, centerY - 150, 200, 30), "Game Paused", textStyle);

            // Create a GUIStyle for the button with white lettering and a custom background
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 0.8f));
            buttonStyle.hover.background = MakeTex(2, 2, new Color(0.3f, 0.3f, 0.3f, 0.8f));

            if (isChangingLevel)
            {
                // Display scene selector
                selectedSceneIndex = GUI.SelectionGrid(new Rect(centerX - 100, centerY - 100, 200, 30 * sceneNames.Length), selectedSceneIndex, sceneNames, 1);

                // Display "Load Scene" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 30 * sceneNames.Length, 200, 30), "Load Scene", buttonStyle))
                {
                    LoadSelectedScene();
                    isChangingLevel = false;
                    isPaused = false;
                }

                // Display "Back" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 60 + 30 * sceneNames.Length, 200, 30), "Back", buttonStyle))
                {
                    isChangingLevel = false;
                }

                /*
                // Display "Save Progress" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 90 + 30 * sceneNames.Length, 200, 30), "Save Progress", buttonStyle))
                {
                    SaveProgress();
                }

                // Display "Load Progress" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 120 + 30 * sceneNames.Length, 200, 30), "Load Progress", buttonStyle))
                {
                    LoadProgress();
                }
                */
            }
            if (!isChangingLevel && !isShowingKeybinds)
            {
                // Display "Change Level" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 30 * sceneNames.Length, 200, 30), "Change Level", buttonStyle))
                {
                    isChangingLevel = true;
                }

                // Display "Keybinds" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 150 + 30 * sceneNames.Length, 200, 30), "Keybinds", buttonStyle))
                {
                    isShowingKeybinds = true;
                }
            }

            if (isShowingKeybinds)
            {
                // Display list of keybinds
                GUI.Label(new Rect(centerX - 100, centerY - 150, 200, 30), "Escape: Pause/Unpause", textStyle);

                // Display "Back" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 180 + 30 * sceneNames.Length, 200, 30), "Back", buttonStyle))
                {
                    isShowingKeybinds = false;
                }
            }
            /*
            else
            {
                // Display "Change Level" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 30 * sceneNames.Length, 200, 30), "Change Level", buttonStyle))
                {
                    isChangingLevel = true;
                }
                // Display "Keybinds" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 150 + 30 * sceneNames.Length, 200, 30), "Keybinds", buttonStyle))
                {
                    isShowingKeybinds = true;
                }

                if (isShowingKeybinds)
                {
                    // Display list of keybinds
                    GUI.Label(new Rect(centerX - 100, centerY - 150, 200, 30), "Escape: Pause/Unpause", textStyle);

                    // Display "Back" button
                    if (GUI.Button(new Rect(centerX - 100, centerY + 180 + 30 * sceneNames.Length, 200, 30), "Back", buttonStyle))
                    {
                        isShowingKeybinds = false;
                    }
                }
                
            }
            */
        }
    }

    // Helper method to create a Texture2D with a single color
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private void LoadSelectedScene()
    {
        if (selectedSceneIndex >= 0 && selectedSceneIndex < sceneNames.Length)
        {
            // Unpause the game before loading the new scene
            Time.timeScale = 1;
            SceneManager.LoadScene(selectedSceneIndex);
        }
    }

    /* Save and load progress
    private void SaveProgress()
    {
        // Save the current scene index
        PlayerPrefs.SetInt("SavedSceneIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        // Check if a saved scene index exists
        if (PlayerPrefs.HasKey("SavedSceneIndex"))
        {
            // Load the saved scene
            SceneManager.LoadScene(PlayerPrefs.GetInt("SavedSceneIndex"));

            // Unpause the game
            Time.timeScale = 1;
        }
    }
    */
}