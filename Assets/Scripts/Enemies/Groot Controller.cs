using UnityEngine;
public class GrootController : EnemyBase
{
    protected override void Update()
    {
        Move();
        FlipSprite();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moveSpeed = -moveSpeed;
        }
    }
}