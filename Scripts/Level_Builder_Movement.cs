using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Builder_Movement : MonoBehaviour
{

    public float minSpeed;
    public float maxSpeed;
    public float timeToMaxSpeed;
    private float timeAccelerating;
    
    // Private physics components.
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Get and store physics component.
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        
        // Allows the player to gradually accelerate when holding a movement button.
        float motionAmount = Mathf.Lerp(minSpeed * Time.deltaTime, maxSpeed * Time.deltaTime, timeAccelerating / timeToMaxSpeed);

        if(up || down || left || right){
            timeAccelerating += Time.deltaTime;
        }else{
            timeAccelerating = 0;
        }

        if(up){
            rb.position = new Vector2(rb.position[0], rb.position[1] + motionAmount);
        }
        if(down){
            rb.position = new Vector2(rb.position[0], rb.position[1] - motionAmount);
        }
        if(left){
            rb.position = new Vector2(rb.position[0] - motionAmount, rb.position[1]);
        }
        if(right){
            rb.position = new Vector2(rb.position[0] + motionAmount, rb.position[1]);
        }
    }
}
