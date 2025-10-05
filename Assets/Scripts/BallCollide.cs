using UnityEngine;

public class BallCollide : MonoBehaviour
{
    // Referenced components
    public Transform ballTransform;
    public Rigidbody2D rigidBody;
    public BallMove ballMove;

    // Constants, set in game engine
    public float boostSpeed;        // Speed boost panels attempt to apply in their direction
    public float rampBoostSpeed;    // Speed ramps attempt to apply in their direction
    public float rampHeight;        // Peak "height" ramps give the ball, correlates to collision ignore time
    public float greenDrag;         // Drag applied when ball is on the default terrain
    public float roughDrag;         // Drag applied when ball is on rough terrain
    public float sandDrag;          // Drag applied when ball is on sand
    public float iceDrag;           // Drag applied when ball is on ice
    public float noTerrainDrag;     // Drag applied when ball is in the "air" or over out of bounds

    // Instance variables
    LayerMask floorLayers;
    LayerMask specialLayers;

    void Start()
    {
        floorLayers = LayerMask.GetMask("Green", "Rough", "Sand", "Ice");
        specialLayers = LayerMask.GetMask("Hole", "Ramp", "Boost", "OutOfBounds");
    }

    void Update()
    {
        updateTimers();
        runFloorChecks();
        runSpecialFloorChecks();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // TODO not sure if I will end up using this, maybe can for everything except for hole and oob?
    }

    private void updateTimers()
    {

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
            // TODO height logic
            Vector2 boostVector = collider.transform.right * rampBoostSpeed;
            ballMove.applyBoost(boostVector);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Boost"))
        {
            Vector2 boostVector = collider.transform.right * boostSpeed;
            ballMove.applyBoost(boostVector);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("OutOfBounds"))
        {
            Debug.Log("Collided with out of bounds");
            // TODO call handleOutOfBounds in BallMove
        }
    }
    
    // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ball"), LayerMask.NameToLayer("Walls"), true) -- use this on ramp activation
}
