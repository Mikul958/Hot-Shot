using UnityEngine;
using UnityEngine.InputSystem;

public class BallMove : MonoBehaviour
{
    // Referenced components
    public Rigidbody2D rigidBody;
    public LineRenderer inputTrailRenderer;

    // Constants, set in game engine
    public float minMouseDiff;      // Minimum drag distance required for a hit.
    public float maxMouseDiff;      // Maximum effective drag distance, further drag will not affect force.

    public float minHitSpeed;       // Speed applied to the ball at a drag distance of minMouseDiff.
    public float maxHitSpeed;       // Speed applied to the ball at a drag distance of maxMouseDiff.
    public float maxSafeSpeed;  // If the ball is moving above this speed, inputs are disabled.

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
        if (rigidBody.linearVelocity.magnitude > maxSafeSpeed)
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
            inputTrailRenderer.enabled = true;
            Debug.Log("Clicked ball -- Raycast hit at position: " + mousePos);
        }
    }

    private void checkForBallRelease()
    {
        // If not released, update mouse position and input trail
        if (Mouse.current.leftButton.isPressed)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            updateInputTrail();
            return;
        }

        // Else, hit ball and notify level manager
        inputTrailRenderer.enabled = false;
        Vector2 resultVelocity = mapMouseDifferenceToVelocity(mousePos - rigidBody.position);
        if (resultVelocity != Vector2.zero)
        {
            Debug.Log("Added a stroke to counter");  // TODO notify level manager
            respawnPos = rigidBody.position;
            rigidBody.linearVelocity += resultVelocity;
        }
    }

    private void updateInputTrail()
    {
        // TODO this is all test code
        Vector2 mouseDiff = mousePos - rigidBody.position;
        mouseDiff.Normalize();

        inputTrailRenderer.SetPosition(0, Vector3.zero);
        inputTrailRenderer.SetPosition(1, new Vector3(mouseDiff.x, mouseDiff.y, 0) * -5);
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

    public void handleOutOfBounds()
    {
        // TODO slow down ball + make invisible temporarily, call from BallCollide

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.position = respawnPos;

        // TODO respawn timer stuff
    }
}
