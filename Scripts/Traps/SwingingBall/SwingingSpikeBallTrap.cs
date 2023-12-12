using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingSpikeBallTrap : MonoBehaviour
{
    public float rotationSpeed = 125f;

    // Update is called once per frame
    void Update()
    {
        // Apply the rotation to the parent object
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}