using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Private physics components.
    private Rigidbody2D rb;

    // How many seconds after leaving the ground that the player can still jump. Reminder; By default, 60 frames are in a second.
    public float coyoteTime = 5/60;
    
    // Keeps track of whether the player can currently jump, based on when they last hit another object.
    public bool canJump = false;
    private float coyoteCount = 0;

    public float jumpVelo = 5;
    public float motionSpeed = 2;

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

    }

    

    // Update is called once per frame
    void Update()
    {
        bool jump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);
        bool heldJump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);

        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        
        if (jump && /*!heldJump &&*/ coyoteCount <= coyoteTime){
            rb.velocity = new Vector2(rb.velocity[0], jumpVelo);
            //rb.AddForce(jumpVelo * this.transform.up, ForceMode2D.Impulse);
        }

        if (left && !right){
            rb.velocity = new Vector2(-motionSpeed, rb.velocity[1]);
        }

        if (!left && right){
            rb.velocity = new Vector2(motionSpeed, rb.velocity[1]);
        }

        if (touching.Count == 0) {
            coyoteCount += Time.deltaTime;
            
        }/*else{
            coyoteCount = 0;
        }*/
    }
}
