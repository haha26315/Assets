using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Call the CompleteLevel method
            gameManager.CompleteLevel();
        }
    }
}