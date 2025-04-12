public class GrootController : EnemyBase
{
    protected override void Update()
    {
        Move();
        FlipSprite();
    }
}
