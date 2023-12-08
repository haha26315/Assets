using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Private physics components.
    private Rigidbody2D rb;

    // How many seconds after leaving the ground that the player can still jump. Reminder; By default, 60 frames are in a second.
    public float coyoteTime = 10/60;
    
    // Keeps track of whether the player can currently jump, based on when they last hit another object.
    public bool canJump = false;
    private float coyoteCount = 0;

    public float jumpVelo = 5;
    public float motionSpeed = 2;

    // Whether or not a specific movement ability is activated for a level.
    public bool dashActivated = true;
    public bool grappleActivated = true;
    public bool teleportActivated = true;

    // Dashes refresh upon touching the ground like with jumps.
    public bool canDash = true;
    public float dashForce = 10;

    // How long the player can hold on while grappling
    public float grappleTime = 2;
    private float grappleTimeCount = 0;

    // How long until the grapple refreshes
    public float grappleCooldown = 5;
    private float grappleCooldownCount = 0;

    // How far away the grapple can reach
    public float grappleMaxRange = 5;

    // Where the end of the grapple is. Useful for adding speed.
    private Vector2 grappleEnd;
    
    // How long until teleportation refreshes
    public float teleportCooldown = 10;

    // Slightly janky way of determining if there are *no* collisions this frame, to determine things like coyoteCount. Count if its size is zero or not.
    HashSet<Collision2D> touching = new HashSet<Collision2D>();

    // Fires upon player trigger entry.
    private void OnCollisionEnter2D(Collision2D c)
    {
        // Get object associated with collision.
        GameObject Collider = c.gameObject;

        touching.Add(c);

        //Refresh the player's jump.
        coyoteCount = 0;
        
        // TODO; Make sure that bonking a ceiling doesn't refresh the jump 
        // by getting the position of the colliding object and determining 
        // with the vector if it hit the top vs the side of the player. 
        // Partway easy with an angle check.
        canJump = true;

        canDash = true;

        // Code from my last unity project that may be useful as a reference later.

        // Checks what type of object it's hit before acting.
        /*if (Collider.CompareTag("Projectile"))
        {
            // Creates an explosion where the planet is.
            Destroy(Instantiate(ExplosionPrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation), 1.25f);
            Health -= 25f;
            
            // Updates player health UI.
            UIHelper.SetPlayerHealth(Health);
        }
        else if (Collider.CompareTag("Sun"))
        {
            // Creates an explosion at the location of the planet.
            GameObject.Destroy(Instantiate(ExplosionPrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation), 1.25f);

            // Removes this from the list of objects to calculate gravity for.
            GravityObjects.Remove(this.gameObject);

            if (CurrentBlackHole)
            {
                // Removes black hole from the list of objects to calculate gravity for.
                GravityObjects.Remove(CurrentBlackHole);

                // Deletes black hole.
                GameObject.Destroy(CurrentBlackHole);
            }
            
            // Sets health variable for UI.
            //UIHelper.SetPlayerHealth(0f);

            // Destroys this planet.
            //GameObject.Destroy(this.gameObject);
        }
        */
    }

    void OnCollisionExit2D(Collision2D c) {
        touching.Remove(c);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get and store physics component.
        rb = GetComponent<Rigidbody2D>();

        // Make sure player can grapple when they spawn.
        grappleCooldownCount = grappleCooldown;
    }

    

    // Update is called once per frame
    void Update()
    {
        bool jump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);
        bool heldJump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);

        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        bool down = Input.GetKey(KeyCode.S);
        
        bool shift = Input.GetKeyDown(KeyCode.LeftShift); // Controls dashing
        //bool shiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        bool leftMouse = Input.GetMouseButton(0); // Controls grappling
        bool leftMouseDown = Input.GetMouseButtonDown(0);
        bool rightMouse = Input.GetMouseButtonDown(1); // Controls teleporting

        Vector3 mousePos = Input.mousePosition;
        // Convert absolute mouse coordinates into world mouse coordinates.
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector3 grappleEnd = new Vector3(0,0,0);

        if (shift && canDash){
            
            int numKeysHeld = 0;
            if(left){
                numKeysHeld++;
            }
            if(right){
                numKeysHeld++;
            }
            if(heldJump){
                numKeysHeld++;
            }
            if(down){
                numKeysHeld++;
            }

            if(left){
                rb.AddForce(Vector3.left * dashForce / numKeysHeld);
            }
            if(right){
                rb.AddForce(Vector3.right * dashForce / numKeysHeld);
            }
            if(heldJump){
                rb.AddForce(Vector3.up * dashForce / numKeysHeld);
            }
            if(down){
                rb.AddForce(Vector3.down * dashForce / numKeysHeld);
            }
            if(numKeysHeld > 0){
                canDash = false;
            }
        }

        if(rightMouse){

            // Teleport the player to the position of the mouse.
            transform.position = mousePos;

            // Delete velocity on teleport
            rb.velocity = new Vector2(0, 0);
        }
        
        if (jump && /*!heldJump &&*/ coyoteCount <= coyoteTime){

            if(rb.velocity[1] <= jumpVelo){
                rb.AddForce(jumpVelo * Vector3.up, ForceMode2D.Impulse);
            }
            //rb.velocity = new Vector2(rb.velocity[0], jumpVelo);
            //rb.AddForce(jumpVelo * this.transform.up, ForceMode2D.Impulse);
        }

        bool isGrappling = leftMouse && grappleCooldownCount >= grappleCooldown && grappleTimeCount <= grappleTime;
        if(!isGrappling){
            grappleCooldownCount += Time.deltaTime;
            grappleTimeCount = 0;
        }else if(leftMouseDown){
            
            grappleEnd = mousePos;
            grappleTimeCount = 0;
            //transform.rotation = Vector2.Angle(transform.position, grappleEnd);
        }else{
            grappleTimeCount += Time.deltaTime;
            Vector3 offset = grappleEnd - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, offset) * Quaternion.Euler(0,0,90);

            Debug.Log("Grappling");
            //transform.rotation = Vector2.Angle(transform.position, grappleEnd);
        }

        if (left && !right){
            if (!isGrappling){ // The player is not grappling.
                //rb.velocity = new Vector2(-motionSpeed, rb.velocity[1]);
                if(rb.velocity[0] >= -motionSpeed){
                    rb.AddForce(jumpVelo * Vector3.left, ForceMode2D.Impulse);
                }
            }else{ // The player is grappling.
            
                // Get location of grapple end. Add to velocity based on tangent to circle.
                //rb.velocity = new Vector2(transform.forward * motionSpeed);
                rb.AddForce(transform.up * motionSpeed);
            }
        }
        if (!left && right){
            if (!isGrappling){ // The player is not grappling.
                //rb.velocity = new Vector2(motionSpeed, rb.velocity[1]);
                if(rb.velocity[0] <= motionSpeed){
                    rb.AddForce(jumpVelo * Vector3.right, ForceMode2D.Impulse);
                }
            }else{ // The player is grappling.
                
                // Get location of grapple end. Add to velocity based on tangent to circle.
                //rb.velocity = new Vector2(transform.forward * motionSpeed);
                rb.AddForce(-transform.up * motionSpeed);
            }
        }

        if (touching.Count == 0) {
            coyoteCount += Time.deltaTime;
        }
    }
}