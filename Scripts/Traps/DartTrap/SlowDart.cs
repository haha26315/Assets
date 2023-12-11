using System.Collections;
using UnityEngine;

public class SlowDart : MonoBehaviour
{
    private Rigidbody2D playerRb; // The player's rigidbody
    private float originalSpeed; // The player's original speed
    private Rigidbody2D rb; // The dart's rigidbody
    private float maxSpeed = 50f; // The maximum speed of the dart
    private float slowDuration = 5f; // The duration of the slow effect
    private bool hasHitPlayer = false; // Flag to track if the dart has hit the player

    void Start()
    {
        // Get the dart's rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Set the dart's collision detection mode to continuous
        // The dart was going through walls and killing the player because it was moving to fast for the physics engine to detect collisions
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    void Update()
    {
        // Check if the dart's speed exceeds the maximum speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            // Set the dart's speed to the maximum speed
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the slow projectile has collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hasHitPlayer)
            {
                // The dart has already hit the player, so return early
                return;
            }

            // Get the player's rigidbody
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Check if the dart is still moving
                if (rb.velocity.magnitude > 1.0f)
                {
                    // Store the player's original speed
                    originalSpeed = playerRb.velocity.magnitude;
                    Debug.Log("Original speed: " + originalSpeed); // Log the original speed

                    // Halve the player's speed
                    playerRb.velocity *= 0.5f;
                    Debug.Log("Halved speed: " + playerRb.velocity.magnitude); // Log the halved speed

                    // Start the coroutine to reset the player's speed after the slow duration
                    StartCoroutine(ResetSpeedAfterDelay(slowDuration));

                    // Start the coroutine to flash the player's color
                    SpriteRenderer playerSprite = collision.gameObject.GetComponent<SpriteRenderer>();
                    if (playerSprite != null)
                    {
                        StartCoroutine(FlashColor(playerSprite, Color.cyan, slowDuration));
                    }

                    // Set hasHitPlayer to true to prevent the coroutine from being started again
                    hasHitPlayer = true;
                }
            }
        }
    }

    private IEnumerator FlashColor(SpriteRenderer sprite, Color flashColor, float duration)
    {
        Color originalColor = sprite.color;

        // Transition to the flash color
        sprite.color = flashColor;
        yield return new WaitForSeconds(duration / 2);

        // Transition back to the original color
        sprite.color = originalColor;
        yield return new WaitForSeconds(duration / 2);

        Destroy(gameObject);
    }

    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset the player's speed
        playerRb.velocity = playerRb.velocity.normalized * originalSpeed;
        Debug.Log("Reset speed: " + playerRb.velocity.magnitude); // Log the reset speed
    }
}