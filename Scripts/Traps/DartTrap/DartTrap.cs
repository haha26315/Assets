using UnityEngine;

public class DartTrap : MonoBehaviour
{
    public Projectile projectileType;
    public float fireInterval = 2f; // Interval between firing, adjustable in the inspector
    private float timer = 0f; // Timer to keep track of time since last projectile was fired

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Increment timer by the time since last frame
        Debug.Log("Timer: " + timer); // Log the value of timer

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
            }
        }
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
            GameObject projectile = Instantiate(projectileType.projectilePrefab, transform.position + (Vector3)(direction * 0.5f), Quaternion.identity);
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

            switch (projectileType.behavior)
            {
                case Projectile.ProjectileBehavior.Bouncy:
                    // Add bouncy behavior to the projectile
                    //col.sharedMaterial = bouncyMaterial;
                    break;
                case Projectile.ProjectileBehavior.ArrowLike:
                    // Add arrow-like behavior to the projectile
                    rb.gravityScale = 1;
                    break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is a dart
        if (collision.gameObject.CompareTag("Dart"))
        {
            // Destroy the dart when it collides with the player
            if (collision.otherCollider.gameObject.CompareTag("Player"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}