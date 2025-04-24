public class Fish : EnemyBase
{
    protected override void Update()
    {
        Move();
        FlipSprite();
    }
}