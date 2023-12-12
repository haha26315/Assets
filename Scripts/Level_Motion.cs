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

        if (startTheta == 0){
            
            // Ensures this block is pointing at the center before we use its transform for calculations if it's not already rotated.
            //if(transform.rotation == Quaternion.identity){
            //    transform.LookAt(Vector3.zero);
            //}

            Vector2 measureAngleTo = new Vector2(0, 1);
            startTheta = (Vector2.SignedAngle(transform.position, measureAngleTo) / 360f) * (2f * Mathf.PI);

            //Debug.Log("Theta  " + startTheta + "  ActualAngle  "  + Vector2.SignedAngle(transform.position, measureAngleTo));
            Debug.Log("Position  " + transform.position);
        }

        // If halfRadius hasn't been initialized, initialize it to the distance to the center.
        if (halfRadius == 0){
            
            // Meant to work backwards from the influence of our function to then find the proper radius when it's
            // At its lowest point. Doesn't work properly right now at certain positions and I can't for the life of me understand why.
            // Maybe you'll have more success.
            float distanceFromCenter = Mathf.Sqrt(Mathf.Pow(transform.position[0],2) + Mathf.Pow(transform.position[1],2));

            float r = Mathf.Abs(Mathf.Cos(2 * startTheta) * (distanceFromCenter / 2));

            float x = r * Mathf.Sin(startTheta);
            float y = r * Mathf.Cos(startTheta);

            //Debug.Log("R " + r + "  X " + x + "  Y " + y);

            //halfRadius = Mathf.Sqrt(Mathf.Pow(transform.position[0] - x,2) + Mathf.Pow(transform.position[1] - y,2));
            halfRadius = distanceFromCenter - Mathf.Sqrt(Mathf.Pow(x,2) + Mathf.Pow(y,2));
            //Debug.Log("Dist from center " + distanceFromCenter + "  Half Radius  " + halfRadius);
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
