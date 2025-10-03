using UnityEngine;
// Test script to mess with ball bounce in test scene, not to be used anywhere in final game.
public class TestWallCollision : MonoBehaviour
{
    //public Collider2D wallCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Object hit the wall: ");
    }
}