using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Spawner : MonoBehaviour
{
        // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject levelBlockPrefab;
    public float LevelRadius;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {

        for (int i = 0; i < 360; i++){
            // Instantiate at position (0, 0, 0) and zero rotation.
            GameObject currentObj = Instantiate(levelBlockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            currentObj.GetComponent<Level_Motion>().startTheta = i / (360 / (4 * Mathf.PI));
            currentObj.GetComponent<Level_Motion>().halfRadius = (LevelRadius / 2) - 0.5f; // account for width of blocks, etc.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
