using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Motion : MonoBehaviour
{
    private float StartTime;
    public float startTheta;
    public int tenthSecondsToCycle;

    public float halfRadius;

    // Start is called before the first frame update
    void Start()
    {
        StartTime = Time.time;

        // If halfRadius hasn't been initialized, initialize it to the distance to the center.
        if (halfRadius == 0){
            halfRadius = Mathf.Sqrt(Mathf.Pow(transform.position[0],2) + Mathf.Pow(transform.position[1],2));
        }
        if (startTheta == 0){
            startTheta = Vector2.Angle(transform.up, -transform.position) / (360 / (4 * Mathf.PI));
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Define the level's motion in terms of polar coordinates to have an easy means of calculating a closed loop.
        float theta = (((Time.time - StartTime) % tenthSecondsToCycle * 60) / tenthSecondsToCycle) + startTheta;
        float r = Mathf.Abs(Mathf.Cos(2 * theta) * halfRadius) + halfRadius;

        // Convert our polar coordinates into cartesian coordinates
        float x = r * Mathf.Sin(theta);
        float y = r * Mathf.Cos(theta);

        // Set this object to its new coordinates.
        transform.position = new Vector2(x, y);

        // Rotate the level geometry depending on the current angle. Can multiply theta by 2 to get it to look at the middle of each loop in the shape.
        transform.rotation = Quaternion.Euler(Vector3.back * theta * (360 / (2 * Mathf.PI)));

        // Different attempts to make it look more precisely at the middle of each loop in the shape
        /*
        float test = Mathf.Abs(Mathf.Cos(2 * theta) * halfRadius);
        float testx = test * Mathf.Cos(theta);
        float testy = test * Mathf.Cos(theta);
        Vector3 position2 = new Vector3(testx, testy, 0);
        */
        //transform.rotation = Quaternion.Euler(Vector3.up * Vector2.Angle(transform.up, position2 - transform.position) / (360 / (4 * Mathf.PI)));
        //transform.LookAt(Vector2.zero);
    }
}
