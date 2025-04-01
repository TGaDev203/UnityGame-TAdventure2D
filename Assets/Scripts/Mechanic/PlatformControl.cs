using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private Rigidbody2D platformBody;

    // Moving Tile
    [SerializeField] private Transform pointA, pointB;
    [SerializeField] private float moveSpeed;
    private bool movingToB = true;

    // Falling Tile
    [SerializeField] private float fallDelay;
    [SerializeField] private float destroyTime = 3f;
    private bool isFalling = false;

    private void Start()
    {
        platformBody = GetComponent<Rigidbody2D>();

        if (CompareTag("Falling"))
        {
            platformBody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void Update()
    {
        if (CompareTag("Moving") && !isFalling)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        Transform target = movingToB ? pointB : pointA;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            movingToB = !movingToB;
        }
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CompareTag("Falling"))
        {
            isFalling = true;
            Invoke(nameof(StartFalling), fallDelay);
        }
    }

    private void StartFalling()
    {
        platformBody.bodyType = RigidbodyType2D.Dynamic;
        platformBody.gravityScale = 2;
        Destroy(gameObject, destroyTime);
    }
}
