using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public float detonateTimer = 5f; // Interval between firing, adjustable in the inspector

    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    private float timer = 0f; // Timer to keep track of time since last projectile was fired

    public GameObject radiusPrefab; // The radius

    public GameObject explosionParticlesPrefab; // Reference to the particle prefab

    // Create a list to store the players inside the trigger
    List<GameObject> playersInTrigger = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer

        // Instantiate the radius as a child of the grenade
        GameObject radius = Instantiate(radiusPrefab, transform.position, Quaternion.identity, transform);

        // Start the ChangeColorOverTime coroutine
        StartCoroutine(ChangeRadiusOverTime(radius));
    }

    private float maxSpeed = 50f; // The maximum speed of the grenade

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Increment timer by the time since last frame

        if (timer >= detonateTimer) // If detonateTimer seconds have passed since the grenade was fired
        {
            Detonate();
            timer = 0f; // Reset timer
        }
        else
        {
            // Change the color from green to yellow to red as the timer increases
            float fraction = timer / detonateTimer;
            if (fraction < 0.5f)
            {
                spriteRenderer.color = Color.Lerp(Color.green, Color.yellow, fraction * 2f);
            }
            else
            {
                spriteRenderer.color = Color.Lerp(Color.yellow, Color.red, (fraction - 0.5f) * 2f);
            }
        }

        // Get the grenade's rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Check if the grenade's speed exceeds the maximum speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            // Set the grenade's speed to the maximum speed
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Detonate()
    {
        // Get the GameManager component
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();

        // Loop through the players in the trigger
        foreach (GameObject player in playersInTrigger)
        {
            // Check if the GameManager exists
            if (gameManager != null)
            {
                // Run the PlayerDied function
                gameManager.PlayerDied(player);
            }
        }

        // Instantiate the particle system at the grenades's location
        GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
        ParticleSystem particles = explosionParticles.GetComponent<ParticleSystem>();
        particles.Play();

        // Destroy the grenade
        Destroy(gameObject);

        // Destroy the particle system after it has finished playing
        Destroy(explosionParticles, particles.main.duration);
    }

    private IEnumerator ChangeRadiusOverTime(GameObject circle)
    {
        float elapsedTime = 0f;
        CircleCollider2D trigger = GetComponents<CircleCollider2D>().FirstOrDefault(c => c.isTrigger); // Get the trigger
        float finalScale = trigger.radius * 2; // Use the diameter of the trigger as the final scale
        SpriteRenderer circleRenderer = circle.GetComponent<SpriteRenderer>(); // Get the SpriteRenderer of the circle
        float radiusExpansionTime = detonateTimer / 2; // Set the radius expansion time to half of the detonate timer

        while (elapsedTime < radiusExpansionTime)
        {
            // Calculate the fraction of the total time that has passed
            float fraction = elapsedTime / radiusExpansionTime;

            // Change the scale of the circle
            circle.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(finalScale, finalScale, 1), fraction);

            // Change the color of the circle from green to yellow to red, while retaining its current transparency value
            if (fraction < 0.5f)
            {
                // Transition from green to yellow
                circleRenderer.color = Color.Lerp(new Color(Color.green.r, Color.green.g, Color.green.b, circleRenderer.color.a), new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, circleRenderer.color.a), fraction * 2f);
            }
            else
            {
                // Transition from yellow to red
                circleRenderer.color = Color.Lerp(new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, circleRenderer.color.a), new Color(Color.red.r, Color.red.g, Color.red.b, circleRenderer.color.a), (fraction - 0.5f) * 2f);
            }

            elapsedTime += Time.deltaTime; // Update the elapsed time
            yield return null; // Wait until the next frame
        }

        // Set the scale of the circle to the final scale when the grenade is about to detonate
        circle.transform.localScale = new Vector3(finalScale, finalScale, 1);

        // Set the color of the circle to red, while retaining its current transparency value
        circleRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.b, circleRenderer.color.a);
    }

    private IEnumerator ChangeColorOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < detonateTimer)
        {
            // Calculate the fraction of the total time that has passed
            float fraction = elapsedTime / detonateTimer;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Add the player to the kill list
            playersInTrigger.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exited object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Remove the player from the kill list
            playersInTrigger.Remove(other.gameObject);
        }
    }
}
