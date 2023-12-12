using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Handles the tip of the spike
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the spike has collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the spike
            //Destroy(gameObject);

            // Get the GameManager component and run the PlayerDied function
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.PlayerDied();
            }
        }
    }

    //Handles the entire spike
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the spike has collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the GameManager component and run the PlayerDied function
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.PlayerDied();
            }
        }
    }
    */
}
