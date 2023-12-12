using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Builder_Movement : MonoBehaviour
{

    public float minSpeed;
    public float maxSpeed;
    public float timeToMaxSpeed;
    private float timeAccelerating;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        
        float motionAmount;
        // Allows the player to gradually accelerate when holding a movement button.
        //Debug.Log("Firing!!! " + Time.deltaTime);
        //if(Time.deltaTime != 0){
        motionAmount = Mathf.Lerp(minSpeed * Time.deltaTime, maxSpeed * Time.deltaTime, timeAccelerating / timeToMaxSpeed);
        //}else{
        //    Debug.Log("Firing!!!");
        //    motionAmount = Mathf.Lerp(minSpeed, maxSpeed, timeAccelerating / timeToMaxSpeed);
        //    timeAccelerating += (1/60);
        //}

        if(up || down || left || right){
            timeAccelerating += Time.deltaTime;
        }else{
            timeAccelerating = 0;
        }

        if(up){
            transform.Translate(0, motionAmount, 0);
        }
        if(down){
            transform.Translate(0, -motionAmount, 0);
        }
        if(left){
            transform.Translate(-motionAmount, 0, 0);
        }
        if(right){
            transform.Translate(motionAmount, 0, 0);
        }
    }
}
