using UnityEngine;

// Test script to mess with hole collisions
public class TestHoleCollision : MonoBehaviour
{
    public Collider2D holeCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure it's a trigger
        if (holeCollider != null && !holeCollider.isTrigger)
        {
            holeCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ball sunk in the hole!");
        // Check if the ball enters the hole
        /*if (other.CompareTag("Ball"))
        {
            Debug.Log("Ball sunk in the hole!");

            // Stop ball movement
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Optionally disable the ball
            other.gameObject.SetActive(false);

            // TODO: Add win condition or next level logic here
            // GameManager.Instance.NextLevel();
        }
        */
    }
}