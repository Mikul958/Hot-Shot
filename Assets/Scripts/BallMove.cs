using UnityEngine;
using UnityEngine.InputSystem;

public class BallMove : MonoBehaviour
{
    // Referenced components
    public Rigidbody2D rigidBody;
    public LineRenderer inputTrailRenderer;

    // Constants, set in game engine
    public float maxSafeSpeed;      // If the ball is moving above this speed, inputs are disabled.
    public float minMouseDiff;      // Minimum drag distance required for a hit.
    public float maxMouseDiff;      // Maximum effective drag distance, further drag will not affect force.

    public float minHitSpeed;       // Speed applied to the ball at a drag distance of minMouseDiff.
    public float maxHitSpeed;       // Speed applied to the ball at a drag distance of maxMouseDiff.
    public float trailStart;        // Distance from the center of the ball the input trail starts.
    public float maxTrailEnd;       // End of the input trail at a drag distance of maxMouseDiff.

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
            Debug.Log("Clicked ball -- Raycast hit at position: " + mousePos);
        }
    }

    private void checkForBallRelease()
    {
        // If not released, update mouse position and input trail
        if (Mouse.current.leftButton.isPressed)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            updateInputTrail(false);
            return;
        }

        // Else, hit ball and notify level manager
        updateInputTrail(true);
        Vector2 resultVelocity = mapMouseDifferenceToVelocity(mousePos - rigidBody.position);
        if (resultVelocity != Vector2.zero)
        {
            Debug.Log("Added a stroke to counter");  // TODO notify level manager
            respawnPos = rigidBody.position;
            rigidBody.linearVelocity += resultVelocity;
        }
    }

    private void updateInputTrail(bool destroy)
    {
        // Check mouseDiff length is large enough to apply velocity
        Vector2 mouseDiff = mousePos - rigidBody.position;
        if (destroy || mouseDiff.magnitude <= minMouseDiff)
        {
            inputTrailRenderer.enabled = false;
            return;
        }

        // Calculate start and endpoints of trail given mouseDiff
        if (mouseDiff.magnitude > maxMouseDiff)
            mouseDiff = mouseDiff.normalized * maxMouseDiff;
        float trailLength = (mouseDiff.magnitude - minMouseDiff) * maxTrailEnd / (maxMouseDiff - minMouseDiff) + trailStart;

        mouseDiff = -1 * mouseDiff.normalized;
        Vector3 trailEndPos = new Vector3(mouseDiff.x * trailLength, mouseDiff.y * trailLength, 0);
        Vector3 trailStartPos = new Vector3(mouseDiff.x * trailStart, mouseDiff.y * trailStart, 0);

        // Apply to LineRenderer and enable (or keep enabled if already enabled)
        inputTrailRenderer.SetPosition(0, trailStartPos);
        inputTrailRenderer.SetPosition(1, trailEndPos);
        inputTrailRenderer.enabled = true;
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
