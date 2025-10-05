using UnityEngine;

public class BallCollide : MonoBehaviour
{
    // Referenced components
    public Transform ballTransform;
    public Rigidbody2D rigidBody;

    // Constants, set in game engine
    public float greenDrag;
    public float roughDrag;
    public float sandDrag;
    public float iceDrag;
    public float waterDrag;

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
        runNormalFloorChecks();
        runSpecialFloorChecks();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        
    }

    private void updateTimers()
    {

    }

    private void runNormalFloorChecks()
    {
        Collider2D collider = Physics2D.OverlapPoint(ballTransform.position, floorLayers);
        if (collider == null)
            rigidBody.linearDamping = 0f;
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
            Debug.Log("Collided with a ramp");
            // TODO call applyBoost in BallMove based on boost panel direction
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Boost"))
        {
            Debug.Log("Collided with a boost panel");
            // TODO call applyBoost in BallMove based on boost panel direction
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("OutOfBounds"))
        {
            Debug.Log("Collided with out of bounds");
            rigidBody.linearDamping = waterDrag;
            // TODO call handleOutOfBounds in BallMove
        }
    }
    
    // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ball"), LayerMask.NameToLayer("Walls"), true) -- use this on ramp activation
}
