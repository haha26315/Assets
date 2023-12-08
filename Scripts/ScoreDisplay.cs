using UnityEngine;
using TMPro; // Namespace for TextMeshPro

public class ScoreDisplay : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script
    private TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component

    void Start()
    {
        // Get the TextMeshProUGUI component
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Update the text to display the player's score
        scoreText.text = "Score: " + gameManager.score;
    }
}