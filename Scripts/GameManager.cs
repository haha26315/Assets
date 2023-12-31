using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab; // Reference to the player prefab
    public Vector2 startPosition; // The position where the player should respawn

    private GameObject currentPlayer; // Reference to the current player object

    // For the camera
    public float zoomSpeed = 1f;
    public float manualZoomFactor = 5f;
    public float followSpeed = 10f;
    public float minZoom = 5f;
    private float currentMinZoom;
    public float maxZoom = 15f;
    public float speedToMaxZoom = 30f;
    public bool idleZoom = true;

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
        currentMinZoom = minZoom;

        // Instantiate the player at the start of the game
        currentPlayer = Instantiate(playerPrefab, startPosition, Quaternion.identity);
        playerRigidbody = currentPlayer.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get the name of the current scene
        string levelName = SceneManager.GetActiveScene().name;

        // Get the high score for the current level
        int highScore = PlayerPrefs.GetInt(levelName + "HighScore", 0);

        // Check if the score has increased
        if (score > highScore)
        {
            // Update the high score
            PlayerPrefs.SetInt(levelName + "HighScore", score);
        }
        
        // Make the camera follow the player
        Vector3 targetPosition = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, cam.transform.position.z);
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, followSpeed * Time.deltaTime);

        // Get the scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the camera's zoom level based on the scroll wheel input
        currentMinZoom -= scrollInput * zoomSpeed * manualZoomFactor;
        currentMinZoom = Mathf.Clamp(currentMinZoom, minZoom, maxZoom);

        // Calculate the player's speed
        float speed = 0;
        if(playerRigidbody != null){
            speed = playerRigidbody.velocity.magnitude;
        }

        // Checks for if zooming the camera out when idle is enabled.
        if(idleZoom){

            // If the player is not moving and zooming while idle is enabled, increment the timer
            if (speed <= 0.1f && idleZoom)
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

                // Sets the new zoom to be an interpolation based on zoom speed and also the player's speed.
                // As the player gets faster up to our max zoom speed value, the camera zooms out.
                float newZoom = Mathf.Lerp(cam.orthographicSize, Mathf.Lerp(currentMinZoom, maxZoom, Mathf.Min(speed / speedToMaxZoom, speedToMaxZoom)), Time.deltaTime * zoomSpeed);

                //float newZoom = Mathf.Lerp(cam.orthographicSize, minZoom, (Time.deltaTime * zoomSpeed) / speed);
                cam.orthographicSize = newZoom;
            }
        }else{
            // We're probably editing a level, so zooming a level is only based upon scrolling
            // No smoothing to allow for easier, more precise selection of objects
            cam.orthographicSize = currentMinZoom;
        }

    }

    // Can add if score is 0, restart the level.
    public void PlayerDied(GameObject deadPlayer)
    {
        // Decrease the score when the player dies
        score -= scoreDecrease;

        // Ensure the score doesn't go below zero
        score = Mathf.Max(score, 0);

        // Create a new ParticleSystem
        GameObject particleSystemObject = new GameObject("DeathParticles");
        particleSystemObject.transform.position = deadPlayer.transform.position;
        ParticleSystem deathParticles = particleSystemObject.AddComponent<ParticleSystem>();

        // Configure the ParticleSystem
        var main = deathParticles.main;
        main.startColor = deadPlayer.GetComponent<SpriteRenderer>().color;
        main.startSize = 0.2f;
        main.startLifetime = 1.0f;
        main.startSpeed = 5.0f;

        var emission = deathParticles.emission;
        emission.rateOverTime = 100;

        var shape = deathParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;

        // Create a new Material using the Sprites-Default shader
        Material particleMaterial = new Material(Shader.Find("Sprites/Default"));
        particleSystemObject.GetComponent<ParticleSystemRenderer>().material = particleMaterial;

        // Play the ParticleSystem
        deathParticles.Play();

        // Stop the ParticleSystem's emission after 1 second
        StartCoroutine(StopEmissionAfterSeconds(deathParticles, 0.3f));

        // Destroy the player object
        deadPlayer.SetActive(false);

        // Start the Respawn coroutine
        StartCoroutine(Respawn(deadPlayer));
    }

    private IEnumerator StopEmissionAfterSeconds(ParticleSystem particleSystem, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        var emission = particleSystem.emission;
        emission.enabled = false;
    }

    private IEnumerator Respawn(GameObject deadPlayer)
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Instantiate a new player object at the start position

        deadPlayer.SetActive(true);
        Rigidbody2D deadPlayerRB = deadPlayer.GetComponent<Rigidbody2D>();
        deadPlayerRB.position = startPosition;
        deadPlayerRB.rotation = 0;

        //currentPlayer = Instantiate(playerPrefab, startPosition, Quaternion.identity);

        // Get the Rigidbody2D from the new player object
        //playerRigidbody = currentPlayer.GetComponent<Rigidbody2D>();
    }

    public void CompleteLevel()
    {
        // Still working on this part

        // Increase the score when the level is completed
        score += scoreIncrease;

        Debug.Log("Level complete! Score: " + score);
    }
}