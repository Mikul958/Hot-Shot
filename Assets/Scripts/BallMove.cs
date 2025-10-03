using UnityEngine;
using UnityEngine.InputSystem;

public class BallMove : MonoBehaviour
{
    // Referenced components
    public Rigidbody2D rigidBody;

    // Constants, set in game engine
    public float minMouseDiff;      // Minimum distance between center of ball and mouse for a hit to happen.
    public float maxMouseDiff;      // Maximum distance between center of ball and mouse, further distances will not affect force.

    public float minHitSpeed;       // Speed applied to the ball at an input distance of minMouseDiff.
    public float maxHitSpeed;       // Speed applied to the ball at an input of maxMouseDiff.
    public float maxSafeThreshold;  // If the ball is moving below this speed, it is considered "safe" and can be hit.

    // Instance variables
    private Vector2 respawnPos;
    private Vector2 mousePos;
    private bool isBallClicked = false;

    void Start() { }

    void Update()
    {
        handlePlayerInput();
    }

    public void handlePlayerInput()
    {
        // Ignore player input if ball is moving too fast
        if (rigidBody.linearVelocity.magnitude > maxSafeThreshold)
            return;

        // Check for user inputs
        if (isBallClicked)
            checkForBallRelease();
        else
            checkForBallClick();
    }

    private void checkForBallClick()
    {
        if (!Mouse.current.leftButton.isPressed)
            return;

        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePos, Vector2.zero);  // TODO may have to switch to RaycastAll if we have overlapping colliders
        if (raycastHit.collider != null && raycastHit.collider.gameObject == gameObject)
        {
            isBallClicked = true;
            Debug.Log("Clicked ball -- Raycast hit at position: " + mousePos);
        }
    }

    private void checkForBallRelease()
    {
        // If not released, update mouse position and 
        if (Mouse.current.leftButton.isPressed)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            updateHitIndicator(true);

            return;
        }

        // Else, hit ball and notify level manager
        updateHitIndicator(false);
        Vector2 resultVelocity = mapMouseDifferenceToVelocity(mousePos - rigidBody.position);
        if (resultVelocity != Vector2.zero)
        {
            Debug.Log("Added a stroke to counter");  // TODO notify level manager
            rigidBody.linearVelocity += resultVelocity;
        }
    }

    private void updateHitIndicator(bool keepActive)
    {
        // TODO figure how to draw the line sprite
    }

    private Vector2 mapMouseDifferenceToVelocity(Vector2 mouseDiff)
    {
        float magnitude = mouseDiff.magnitude;
        if (magnitude <= minMouseDiff)
            return Vector2.zero;
        float resultSpeed = (magnitude - minMouseDiff) * (maxHitSpeed - minHitSpeed) / (maxMouseDiff - minMouseDiff) + minHitSpeed;
        if (resultSpeed > maxHitSpeed)
            resultSpeed = maxHitSpeed;

        Debug.Log("Hit ball at speed: " + resultSpeed);
        return mouseDiff / magnitude * -resultSpeed;  // Normalize mouseDiff vector, then multiply by new speed and invert direction.
    }
}
