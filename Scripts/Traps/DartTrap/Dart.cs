using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the dart's rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Check if the dart is still moving
        if (rb.velocity.magnitude > 1.0f)
        {
            // Check if the dart has collided with the player
            if (collision.gameObject.CompareTag("Player"))
            {
                // Destroy the dart
                Destroy(gameObject);

                // Get the GameManager component and run the PlayerDied function
                GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.PlayerDied();
                }
            }
        }
    }
}