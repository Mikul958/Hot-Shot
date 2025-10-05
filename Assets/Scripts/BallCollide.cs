using UnityEngine;

public class BallCollide : MonoBehaviour
{
    // Referenced components
    public Transform ballTransform;

    // Constants, set in game engine

    // Instance variables
    LayerMask floorLayers;

    void Start()
    {
        floorLayers = LayerMask.GetMask("Hole", "Ramp", "Boost", "Terrain");
    }

    void Update()
    {
        updateTimers();
        runFloorChecks();
    }

    private void updateTimers()
    {
        
    }
    private void runFloorChecks()
    {
        Collider2D collider = Physics2D.OverlapPoint(ballTransform.position, floorLayers);
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
            // TODO call handleOutOfBounds in BallMove
        }
        
        // TODO See if physics material friction can be used for terrain collisions
    }
    
    // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ball"), LayerMask.NameToLayer("Walls"), true) -- use this on ramp activation
}
