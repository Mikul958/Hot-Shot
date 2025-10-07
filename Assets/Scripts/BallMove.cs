using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BallMove : MonoBehaviour
{
    // Referenced components
    public Rigidbody2D rigidBody;
    public SpriteRenderer ballRenderer;
    public LineRenderer inputTrailRenderer;
    private LevelManager levelManager;

    // Frequently-used global constants, obtained from GameConfig
    private float maxSafeSpeed;
    private float minHitSpeed;
    private float maxHitSpeed;
    private float minMouseDiff;
    private float maxMouseDiff;
    private float trailStart;
    private float maxTrailEnd;

    // Instance variables
    private Vector2 mousePos;
    private bool inputEnabled = true;
    private bool isBallClicked = false;
    private Vector2 respawnPos;

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();

        maxSafeSpeed = GameConfig.instance.maxSafeSpeed;
        minHitSpeed = GameConfig.instance.minHitSpeed;
        maxHitSpeed = GameConfig.instance.maxHitSpeed;
        minMouseDiff = GameConfig.instance.minMouseDiff;
        maxMouseDiff = GameConfig.instance.maxMouseDiff;
        trailStart = GameConfig.instance.trailStart;
        maxTrailEnd = GameConfig.instance.maxTrailEnd;
    }

    void Update()
    {
        handlePlayerInput();
    }

    public void handlePlayerInput()
    {
        // Ignore player input if ball is moving too fast
        if (!inputEnabled || rigidBody.linearVelocity.magnitude > maxSafeSpeed)
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
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(mousePos, Vector2.zero);
        foreach (RaycastHit2D hit in raycastHits)
        {
            if (hit.collider.gameObject == gameObject)
                isBallClicked = true;
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
        isBallClicked = false;
        Vector2 resultVelocity = mapMouseDifferenceToVelocity(mousePos - rigidBody.position);
        if (resultVelocity != Vector2.zero)
        {
            levelManager.addStroke();   // Notify level manager that the ball has been hit
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

        mouseDiff.Normalize();
        Vector3 trailEndPos = new Vector3(-mouseDiff.x * trailLength, -mouseDiff.y * trailLength, 0);
        Vector3 trailStartPos = new Vector3(-mouseDiff.x * trailStart, -mouseDiff.y * trailStart, 0);

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

        return mouseDiff / magnitude * -resultSpeed;  // Normalize mouseDiff vector, then multiply by new speed and invert direction.
    }

    public void applyBoost(Vector2 direction, float boostSpeed)
    {
        Vector2 boostVector = direction * boostSpeed;
        float currentSpeedInBoostDir = Vector2.Dot(rigidBody.linearVelocity, boostVector) / boostSpeed;

        if (currentSpeedInBoostDir + boostSpeed < boostSpeed)
            rigidBody.linearVelocity += boostVector;
        else if (currentSpeedInBoostDir < boostSpeed)
            rigidBody.linearVelocity += boostVector * (boostSpeed - currentSpeedInBoostDir) / boostSpeed;
    }

    public void hideBall()
    {
        ballRenderer.enabled = false;
        rigidBody.linearVelocity = Vector2.zero;
        inputEnabled = false;
    }
    public void respawnBall()
    {
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.position = respawnPos;
        ballRenderer.enabled = true;
        inputEnabled = true;
    }
}
