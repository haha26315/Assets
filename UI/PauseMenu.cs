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

            // Disable the controls of the player while paused, re-enable them when unpaused
            // Made this capable of gathering collections in case there is a level
            // with multiple controllable player objects.
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players){

                // If we're paused, controls are disabled.
                player.GetComponent<Player_Movement>().controlsDisabled = isPaused;
            }
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
            }
            if (!isChangingLevel && !isShowingKeybinds)
            {
                // Display "Keybinds" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 30 * sceneNames.Length, 200, 30), "Keybinds", buttonStyle))
                {
                    isShowingKeybinds = true;
                }

                // Display "Change Level" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 60 + 30 * sceneNames.Length, 200, 30), "Change Level", buttonStyle))
                {
                    isChangingLevel = true;
                }

                // Display "Main Menu" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 90 + 30 * sceneNames.Length, 200, 30), "Main Menu", buttonStyle))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene("MainMenu");
                }
            }

            if (isShowingKeybinds)
            {
                // Array of keybinds
                string[] keybinds = new string[10]
                {
                    "Escape: Pause/Unpause",
                    "Keybind 2",
                    "Keybind 3",
                    "Keybind 4",
                    "Keybind 5",
                    "Keybind 6",
                    "Keybind 7",
                    "Keybind 8",
                    "Keybind 9",
                    "Keybind 10"
                };

                // Display list of keybinds
                for (int i = 0; i < keybinds.Length; i++)
                {
                    GUI.Label(new Rect(centerX - 100, centerY - 100 + 30 * i, 200, 30), keybinds[i], textStyle);
                }

                // Display "Back" button
                if (GUI.Button(new Rect(centerX - 100, centerY + 180 + 30 * sceneNames.Length, 200, 30), "Back", buttonStyle))
                {
                    isShowingKeybinds = false;
                }
            }
        }
    }

    // Helper method to create a Texture2D with a single color
    public Texture2D MakeTex(int width, int height, Color col)
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
}