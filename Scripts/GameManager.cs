using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab; // Reference to the player prefab
    public Vector3 startPosition; // The position where the player should respawn

    private GameObject currentPlayer; // Reference to the current player object

    // For the camera
    public float zoomSpeed = 1f;
    public float followSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private Rigidbody2D playerRigidbody;

    // The player's score
    public int score = 1000; // Starting score
    public int scoreIncrease = 500; // Score increase for completing a level
    public int scoreDecrease = 100; // Score decrease for dying

    // Add a new private variable for the timer
    private float idleTimer = 0f;
    private float idleZoomDelay = 3f; // The delay before the camera starts zooming out

    void Start()
    {
        cam = Camera.main;

        // Instantiate the player at the start of the game
        currentPlayer = Instantiate(playerPrefab, startPosition, Quaternion.identity);
        playerRigidbody = currentPlayer.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Make the camera follow the player
        Vector3 targetPosition = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, cam.transform.position.z);
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, followSpeed * Time.deltaTime);

        // Calculate the player's speed
        float speed = playerRigidbody.velocity.magnitude;

        // If the player is not moving, increment the timer
        if (speed <= 0.1f)
        {
            idleTimer += Time.deltaTime;

            // If the timer exceeds the delay, start zooming out
            if (idleTimer >= idleZoomDelay)
            {
                float newZoom = Mathf.Lerp(cam.orthographicSize, maxZoom, Time.deltaTime * zoomSpeed);
                cam.orthographicSize = newZoom;
            }
        }
        else
        {
            // If the player is moving, reset the timer and start zooming in
            idleTimer = 0;
            float newZoom = Mathf.Lerp(cam.orthographicSize, minZoom, Time.deltaTime * zoomSpeed);
            cam.orthographicSize = newZoom;
        }

        // Get the scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the camera's zoom level based on the scroll wheel input
        cam.orthographicSize -= scrollInput * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    public void PlayerDied()
    {
        // Decrease the score when the player dies
        score -= scoreDecrease;

        // Ensure the score doesn't go below zero
        score = Mathf.Max(score, 0);

        // Destroy the player object
        Destroy(currentPlayer);

        // Start the Respawn coroutine
        StartCoroutine(Respawn());

        // Can add if score is 0, restart the level.
    }

    private IEnumerator Respawn()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Instantiate a new player object at the start position
        currentPlayer = Instantiate(playerPrefab, startPosition, Quaternion.identity);

        // Get the Rigidbody2D from the new player object
        playerRigidbody = currentPlayer.GetComponent<Rigidbody2D>();
    }

    public void CompleteLevel()
    {
        // Still working on this part

        // Increase the score when the level is completed
        score += scoreIncrease;

        Debug.Log("Level complete! Score: " + score);
    }
}