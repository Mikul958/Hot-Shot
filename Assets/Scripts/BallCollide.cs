using System;
using UnityEngine;
using UnityEngine.Events;

public class BallCollide : MonoBehaviour
{
    // Referenced components
    public Transform ballTransform;
    public Rigidbody2D rigidBody;
    public CircleCollider2D ballCollider;
    public BallMove ballMove;
    private LevelManager levelManager;

    // Frequenly-used global constants, obtained from GameConfig
    private float configAddedRampScale;
    private float configRampHangtime;

    // Instance variables
    private float boostTimer = 0f;
    private int rampState = 0;              // 0 = resting, 1 = rising, 2 = peak, 3 = falling
    private float rampTimer = 0f;           // Used to time each phase of ramp height
    private int outOfBoundsState = 0;       // 0 = safe, 1 = OoB, waiting on trigger, 2 = waiting on respawn
    private float outOfBoundsTimer = 0f;
    private LayerMask floorLayers;
    private LayerMask specialLayers;

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();

        configAddedRampScale = GameConfig.instance.addedRampScale;
        configRampHangtime = GameConfig.instance.rampHangtime;

        floorLayers = LayerMask.GetMask("Green", "Rough", "Sand", "Ice");
        specialLayers = LayerMask.GetMask("Hole", "Ramp", "Boost", "OutOfBounds");
    }

    void Update()
    {
        updateTimers();
        if (rampState == 0 && outOfBoundsState == 0)
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
            float ballScale = 1 + configAddedRampScale * (configRampHangtime - rampTimer) / configRampHangtime;
            transform.localScale = new Vector3(ballScale, ballScale, 1);
        }
        else if (rampState == 3)
        {
            float ballScale = 1 + configAddedRampScale * rampTimer / configRampHangtime;
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
                rampTimer += configRampHangtime;
            }
        }

        // Update out of bounds timers and notify BallMove if needed
        outOfBoundsTimer -= Time.deltaTime;
        if (outOfBoundsTimer <= 0)
        {
            outOfBoundsTimer = 0f;
            if (outOfBoundsState == 1)
            {
                // TODO play a water splash animation / sound?
                ballMove.hideBall();
                outOfBoundsState = 2;
                outOfBoundsTimer += GameConfig.instance.respawnWait;
            }
            else if (outOfBoundsState == 2)
            {
                ballMove.respawnBall();
                outOfBoundsState = 0;
                outOfBoundsTimer += 0.2f;   // Band-aid fix to second out of bounds trigger on respawn lol
            }
        }
    }

    private void runFloorChecks()
    {
        Collider2D collider = Physics2D.OverlapPoint(ballTransform.position, floorLayers);
        if (collider == null)
            rigidBody.linearDamping = GameConfig.instance.noTerrainDrag;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Green"))
            rigidBody.linearDamping = GameConfig.instance.greenDrag;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Rough"))
            rigidBody.linearDamping = GameConfig.instance.roughDrag;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Sand"))
            rigidBody.linearDamping = GameConfig.instance.veryRoughDrag;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Ice"))
            rigidBody.linearDamping = GameConfig.instance.iceDrag;
    }
    private void runSpecialFloorChecks()
    {
        Collider2D collider = Physics2D.OverlapPoint(ballTransform.position, specialLayers);
        if (collider == null)
            return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Hole"))
        {
            initiateHoleEntry();
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Ramp"))
        {
            initiateRampJump();
            ballMove.applyBoost(collider.transform.right, GameConfig.instance.rampBoostSpeed);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Boost") && boostTimer == 0f)
        {
            ballMove.applyBoost(collider.transform.right, GameConfig.instance.boostSpeed);
            boostTimer += GameConfig.instance.boostCooldown;
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("OutOfBounds") && outOfBoundsTimer == 0f)
        {
            initiateOutOfBounds();
        }
    }

    private void initiateHoleEntry()
    {
        // TODO I'll make this look at bit more natural if I have time
        ballMove.hideBall();
        levelManager.endLevel();     // Notify level manager that the level has ended
        this.enabled = false;
    }
    private void initiateRampJump()
    {
        rigidBody.linearDamping = GameConfig.instance.noTerrainDrag;    // Ensure drag from last terrain hit is cleared before collision checks are disabled
        ballCollider.enabled = false;                                   // Disable collision with walls
        rampState = 1;
        rampTimer = configRampHangtime;
    }

    private void initiateOutOfBounds()
    {
        outOfBoundsState = 1;
        outOfBoundsTimer = GameConfig.instance.outOfBoundsWait;
    }
}
