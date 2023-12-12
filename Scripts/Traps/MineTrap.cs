using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrap : MonoBehaviour
{
    public int damage = 10; // Set the damage value
    public GameObject explosionParticlesPrefab; // Reference to the particle prefab

    public float shakeMagnitude = 0.1f; // Set the shake magnitude
    public int shakeCount = 5; // Set the number of shakes
    public float shakeInterval = 0.1f; // Set the interval between shakes

    private Transform cameraTransform; // The camera's transform

    // Start is called before the first frame update
    void Start()
    {

        // Get the camera's transform
        cameraTransform = Camera.main.transform;

        // Define the directions to cast the ray
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        // Initialize the closest hit
        RaycastHit2D closestHit = new RaycastHit2D();
        float closestDistance = float.MaxValue;

        // Cast a ray in each direction
        foreach (Vector2 direction in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

            // If the raycast hit something and it's closer than the previous closest hit
            if (hit.collider != null && hit.distance < closestDistance)
            {
                // Update the closest hit and distance
                closestHit = hit;
                closestDistance = hit.distance;
            }
        }

        // If a hit was found
        if (closestHit.collider != null)
        {
            // Move the mine to the hit point
            transform.position = closestHit.point;

            // Rotate the mine to align with the surface normal
            transform.up = closestHit.normal;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Find the GameManager instance and call the PlayerDied method
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.PlayerDied(other.gameObject);
            }

            // Instantiate the particle system at the mine's location
            GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
            ParticleSystem particles = explosionParticles.GetComponent<ParticleSystem>();
            particles.Play();

            // Destroy the particle system after it has finished playing
            Destroy(explosionParticles, particles.main.duration);

            // Start the Shake coroutine
            StartCoroutine(Shake());

            // Calculate the direction from the mine to the player
            Vector2 explosionDirection = other.transform.position - transform.position;

            // Normalize the direction
            float explosionForce = 10f; // Set the explosion force value

            explosionDirection = explosionDirection.normalized;

            // Apply a force to the player
            Rigidbody2D playerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);
            }

            // Destroy the mine object immediately
            Destroy(gameObject);
        }
    }
    
    IEnumerator Shake()
    {
        Vector3 originalPosition = cameraTransform.localPosition;

        for (int i = 0; i < shakeCount; i++)
        {
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            yield return new WaitForSeconds(shakeInterval);

            cameraTransform.localPosition = originalPosition;
        }
    }
}