using UnityEngine;
using UnityEngine.InputSystem;

public class BallMove : MonoBehaviour
{
    // Referenced components
    public Rigidbody2D rigidBody;

    // Constants, set in game engine
    public float minHitLength;      // Minimum distance between center of ball and mouse for a hit to happen.
    public float maxHitLength;      // Maximum distance between center of ball and mouse, further distances will not affect force.
    public float maxHitVelocity;    // Velocity applied to the ball at an input of maxHitLength. Velocity applied ranges from 0 to maxHitVelocity.
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

        // If clicked, check for position updates and release, otherwise check for a new click
        if (isBallClicked)
            checkForMouseRelease();
        else
            checkForMouseClick();
    }

    private void checkForMouseClick()
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

    private void checkForMouseRelease()
    {
        // TODO if not released, update mousepos and figure out drawing line

        // TODO check for release. if release do vector math to add velocity
    }
}
