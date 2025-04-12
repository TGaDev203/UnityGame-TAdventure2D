using UnityEngine;

public class FishController : EnemyBase
{
    protected override void Update()
    {
        Move();
        FlipSprite();
    }
}
