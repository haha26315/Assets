using System.Collections;
using UnityEngine;

public class DartTrap : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab
    public float fireInterval = 5f; // Interval between firing, adjustable in the inspector
    private float timer = 0f; // Timer to keep track of time since last projectile was fired
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Increment timer by the time since last frame
        //Debug.Log("Timer: " + timer); // Log the value of timer

        if (timer >= fireInterval) // If fireInterval seconds have passed since last projectile was fired
        {
            try
            {
                FireProjectile();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error firing projectile: " + e.Message);
            }
            finally
            {
                timer = 0f; // Reset timer
                StartCoroutine(ChangeColorOverTime()); // Start the color change coroutine
            }
        }
        else
        {
            // Change the color from green to yellow to red as the timer increases
            float fraction = timer / fireInterval;
            if (fraction < 0.5f)
            {
                spriteRenderer.color = Color.Lerp(Color.green, Color.yellow, fraction * 2f);
            }
            else
            {
                spriteRenderer.color = Color.Lerp(Color.yellow, Color.red, (fraction - 0.5f) * 2f);
            }
        }
    }

    private IEnumerator ChangeColorOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fireInterval)
        {
            // Calculate the fraction of the total time that has passed
            float fraction = elapsedTime / fireInterval;

            // Change the color from green to yellow to red
            if (fraction < 0.5f)
            {
                // Transition from green to yellow
                spriteRenderer.color = Color.Lerp(Color.green, Color.yellow, fraction * 2f);
            }
            else
            {
                // Transition from yellow to red
                spriteRenderer.color = Color.Lerp(Color.yellow, Color.red, (fraction - 0.5f) * 2f);
            }

            elapsedTime += Time.deltaTime; // Update the elapsed time
            yield return null; // Wait until the next frame
        }

        // Set the color to red when the trap is about to fire
        spriteRenderer.color = Color.red;
    }

    public void FireProjectile()
    {
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Calculate the direction from the trap to the player
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Calculate the distance to the player
            float distance = Vector2.Distance(transform.position, player.transform.position);

            // Instantiate the projectile in the direction of the player
            GameObject projectile = Instantiate(projectilePrefab, transform.position + (Vector3)(direction * 0.5f), Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Collider2D col = projectile.GetComponent<Collider2D>();

            // Apply force in the direction of the player, scaled by the distance to the player
            float force = 5f * distance; // Force * the distance to the player
            rb.AddForce(direction * force, ForceMode2D.Impulse);

            // Rotate the sprite so that the top of it points in the direction of travel
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Subtract 90 to make the top of the sprite point in the direction of travel

            // Destroy the dart after 10 seconds
            Destroy(projectile, 10f);
        }
    }
}