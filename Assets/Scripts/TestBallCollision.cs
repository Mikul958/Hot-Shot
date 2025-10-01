using UnityEngine;

// Test script to mess with ball bounce in test scene, not to be used anywhere in final game.
public class TestBallCollision : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public float bounceStrength = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody.linearVelocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update() { }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        rigidBody.linearVelocityY = bounceStrength;
        bounceStrength *= 0.75f;

        if (bounceStrength <= 1)
            rigidBody.linearVelocity = new Vector2(10, 5);
    }
}
