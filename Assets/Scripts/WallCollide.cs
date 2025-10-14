using UnityEngine;
using UnityEngine.Tilemaps;

public class WallCollide : MonoBehaviour
{
    public TilemapCollider2D tilemapCollider;
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        FindObjectOfType<AudioManager>().Play("Rock_bounce");
    }
}