using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[System.Serializable]
public class Level
{
    public string sceneName;
    public Sprite image;
}

public class LevelsPanel : MonoBehaviour
{
    public List<Level> levels;
    public Transform levelButtonsParent;
    public Image levelImage;
    public Button loadLevelButton;

    private string selectedLevel;

    // Highlighted colors
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    private Button selectedButton;

    // Duration of the color transition in seconds
    public float transitionDuration = 0.2f;


    void Start()
    {
        // Disable the load level button initially
        loadLevelButton.interactable = false;

        // Create a button for each level
        foreach (Level level in levels)
        {
            // Create a new GameObject for the button container
            GameObject buttonContainer = new GameObject("ButtonContainer");
            buttonContainer.AddComponent<RectTransform>();
            buttonContainer.AddComponent<CanvasRenderer>();
            buttonContainer.AddComponent<LayoutElement>();

            // Create a new GameObject for the button
            GameObject buttonObject = new GameObject("LevelButton");
            buttonObject.transform.SetParent(buttonContainer.transform, false);
            buttonObject.AddComponent<RectTransform>();
            buttonObject.AddComponent<CanvasRenderer>();
            Button levelButton = buttonObject.AddComponent<Button>();
            Image buttonImage = buttonObject.AddComponent<Image>();

            /*
            // Create a new GameObject for the frame
            GameObject frameObject = new GameObject("Frame");
            frameObject.transform.SetParent(buttonContainer.transform, false);
            frameObject.transform.SetAsFirstSibling();
            Image frameImage = frameObject.AddComponent<Image>();
            frameImage.color = normalColor;

            // Set the frame's RectTransform to be slightly larger than the button's
            RectTransform frameRect = frameObject.GetComponent<RectTransform>();
            frameRect.anchorMin = new Vector2(-0.05f, -0.05f); // 5% larger on the bottom left
            frameRect.anchorMax = new Vector2(1.05f, 1.05f); // 5% larger on the top right
            frameRect.anchoredPosition = Vector2.zero;
            frameRect.sizeDelta = Vector2.zero;
            */

            // Set the button container's parent
            buttonContainer.transform.SetParent(levelButtonsParent, false);

            // Create a new GameObject for the text
            GameObject textObject = new GameObject("Text");

            // Add the necessary components
            textObject.AddComponent<RectTransform>();
            textObject.AddComponent<CanvasRenderer>();
            Text buttonText = textObject.AddComponent<Text>();

            // Set the text's parent
            textObject.transform.SetParent(buttonObject.transform, false);

            // Set the button's text and image
            buttonText.text = level.sceneName;
            buttonImage.sprite = level.image;

            // Add a click listener to the button
            levelButton.onClick.AddListener(() => SelectLevel(levelButton, /* frameImage, */ level.sceneName));

            // Add pointer enter and exit listeners
            EventTrigger trigger = buttonObject.AddComponent<EventTrigger>();

            // Pointer enter event
            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((eventData) => 
            {
                // Only change the color to hover if this is not the selected button
                if (levelButton != selectedButton)
                {
                    StartCoroutine(TransitionColor(buttonImage, hoverColor)); //StartCoroutine(TransitionColor(frameImage, hoverColor)); });
                }
            });
            trigger.triggers.Add(pointerEnterEntry);

            // Pointer exit event
            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
            pointerExitEntry.eventID = EventTriggerType.PointerExit;
            pointerExitEntry.callback.AddListener((eventData) => 
            {
                // Only change the color back to normal if this is not the selected button
                if (levelButton != selectedButton)
                {
                    StartCoroutine(TransitionColor(buttonImage, normalColor)); //StartCoroutine(TransitionColor(frameImage, hoverColor)); });
                }
            });
            trigger.triggers.Add(pointerExitEntry);
        }

        // Add click listener to the load level button
        loadLevelButton.onClick.AddListener(LoadLevel);
    }

    IEnumerator TransitionColor(Image image, Color targetColor)
    {
        Color startColor = image.color;
        float startTime = Time.time;

        while (Time.time < startTime + transitionDuration)
        {
            float t = (Time.time - startTime) / transitionDuration;
            image.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        image.color = targetColor;
    }

    void SelectLevel(Button levelButton, /* Image frameImage, */ string levelName)
    {
        // Update the selected level
        selectedLevel = levelName;

        // Update the selected button
        if (selectedButton != null)
        {
            StartCoroutine(TransitionColor(selectedButton.GetComponent<Image>(), normalColor));
            // StartCoroutine(TransitionColor(selectedButton.transform.Find("Frame").GetComponent<Image>(), normalColor));
        }
        selectedButton = levelButton;
        StartCoroutine(TransitionColor(levelButton.GetComponent<Image>(), selectedColor));
        // StartCoroutine(TransitionColor(frameImage, selectedColor));

        // Update the level image
        Sprite levelSprite = levels.Find(level => level.sceneName == levelName).image;
        levelImage.sprite = levelSprite;

        // Enable the load level button
        loadLevelButton.interactable = true;
    }

    void LoadLevel()
    {
        // Load the selected level
        SceneManager.LoadScene(selectedLevel);
    }

    public void GoBack()
    {
        // Show the main menu panel and hide the levels panel
        transform.parent.Find("LevelPanel").gameObject.SetActive(false);
    }
}