using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set the dart's collision detection mode to continuous
        // The dart was going through walls and killing the player because it was moving to fast for the physics engine to detect collisions
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private float maxSpeed = 50f; // The maximum speed of the dart

    void Update()
    {
        // Get the dart's rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Check if the dart's speed exceeds the maximum speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            // Set the dart's speed to the maximum speed
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
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