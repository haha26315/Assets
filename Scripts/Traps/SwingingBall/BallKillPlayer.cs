using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKillPlayer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the ball has collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the ball
            //Destroy(gameObject);

            // Get the GameManager component and run the PlayerDied function
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.PlayerDied(collision.gameObject);
            }
        }
    }
}