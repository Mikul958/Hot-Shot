using System;
using UnityEngine;

public class BallCollide : MonoBehaviour
{
    // Referenced components
    public Transform ballTransform;
    public Rigidbody2D rigidBody;
    public CircleCollider2D ballCollider;
    public BallMove ballMove;

    // Constants, set in game engine
    public float boostSpeed;        // Speed boost panels attempt to apply in their direction
    public float rampBoostSpeed;    // Speed ramps attempt to apply in their direction
    public float rampScale;         // Additional visual scale applied to ball by ramp
    public float rampHangtime;      // Amount of time ball stays at its peak, rise/fall phases take the same amount of time
    public float boostCooldown;     // Minimum time allowed between two boost panel collisions (not ramps)
    public float outOfBoundsWait;   // Time between touching OoB collision and being visibly counted out of bounds
    public float respawnTime;       // Time it takes for the player to respawn after OoB is fully triggered
    public float greenDrag;         // Drag applied when ball is on the default terrain
    public float roughDrag;         // Drag applied when ball is on rough terrain
    public float sandDrag;          // Drag applied when ball is on sand
    public float iceDrag;           // Drag applied when ball is on ice
    public float noTerrainDrag;     // Drag applied when ball is in the "air" or over out of bounds

    // Instance variables
    private float boostTimer = 0f;
    private int rampState = 0;      // 0 = resting, 1 = rising, 2 = peak, 3 = falling
    private float rampTimer = 0f;   // Used to time each phase of ramp height
    private float outOfBoundsTimer = 0f;
    private float respawnTimer = 0f;
    private LayerMask floorLayers;
    private LayerMask specialLayers;

    void Start()
    {
        floorLayers = LayerMask.GetMask("Green", "Rough", "Sand", "Ice");
        specialLayers = LayerMask.GetMask("Hole", "Ramp", "Boost", "OutOfBounds");
    }

    void Update()
    {
        updateTimers();
        if (rampState == 0)
        {
            runFloorChecks();
            runSpecialFloorChecks();
        }
    }

    private void updateTimers()
    {
        // Decrement boostTimer
        boostTimer -= Time.deltaTime;
        if (boostTimer < 0f)
            boostTimer = 0f;

        // Increment / decrement timers and update ball scale
        rampTimer -= Time.deltaTime;
        if (rampTimer < 0)
            rampTimer = 0f;

        if (rampState == 1)
        {
            float ballScale = 1 + rampScale * (rampHangtime - rampTimer) / rampHangtime;
            transform.localScale = new Vector3(ballScale, ballScale, 1);
        }
        else if (rampState == 3)
        {
            float ballScale = 1 + rampScale * rampTimer / rampHangtime;
            transform.localScale = new Vector3(ballScale, ballScale, 1);
        }

        // Handle ramp state changes
        if (rampTimer == 0f)
        {
            if (rampState == 3)
            {
                ballCollider.enabled = true;    // Ball has landed, re-enable collision
                rampState = 0;
            }
            else if (rampState != 0)
            {
                rampState++;
                rampTimer += rampHangtime;
            }
        }

        // Update out of bounds timers and notify BallMove if needed
        // TODO
    }

    private void runFloorChecks()
    {
        Collider2D collider = Physics2D.OverlapPoint(ballTransform.position, floorLayers);
        if (collider == null)
            rigidBody.linearDamping = noTerrainDrag;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Green"))
            rigidBody.linearDamping = greenDrag;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Rough"))
            rigidBody.linearDamping = roughDrag;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Sand"))
            rigidBody.linearDamping = sandDrag;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Ice"))
            rigidBody.linearDamping = iceDrag;
    }
    private void runSpecialFloorChecks()
    {
        Collider2D collider = Physics2D.OverlapPoint(ballTransform.position, specialLayers);
        if (collider == null)
            return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Hole"))
        {
            Debug.Log("Level complete!");
            // TODO notify level manager of completion and do ball velocity stuff
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Ramp"))
        {
            initiateRampJump();
            ballMove.applyBoost(collider.transform.right, rampBoostSpeed);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Boost") && boostTimer == 0f)
        {
            ballMove.applyBoost(collider.transform.right, boostSpeed);
            boostTimer += boostCooldown;
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("OutOfBounds"))
        {
            Debug.Log("Collided with out of bounds");
            // TODO call handleOutOfBounds in BallMove
        }
    }

    private void initiateRampJump()
    {
        rigidBody.linearDamping = noTerrainDrag;    // Ensure drag from last terrain hit is cleared before collision checks are disabled
        ballCollider.enabled = false;               // Disable collision with walls
        rampState = 1;
        rampTimer = rampHangtime;
    }
    
    // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ball"), LayerMask.NameToLayer("Walls"), true) -- use this on ramp activation
}
