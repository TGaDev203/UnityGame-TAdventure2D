public class CrocodileController : EnemyBase
{
    protected override void Update()
    {
        Move();
        FlipSprite();
    }
}
