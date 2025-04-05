using UnityEngine;

public class PlatformControl : MonoBehaviour
{

    // Moving Tile
    [SerializeField] private float tileMovingDistance;
    public float tileMoveSpeed;
    private Vector2 startPos;
    private Vector2 lastPos;
    private bool movingRight = true;
    private bool movingUp = true;

    // Falling Tile
    [SerializeField] private float fallDelay;
    [SerializeField] private float destroyTime;
    private bool isFalling = false;

    private Rigidbody2D playerBody;
    private Rigidbody2D platformBody;
    private bool isOnPlatform = false;

    private void Awake()
    {
        platformBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startPos = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerBody = player.GetComponent<Rigidbody2D>();
        }

        if (CompareTag("Falling"))
        {
            platformBody.bodyType = RigidbodyType2D.Kinematic;
        }

        lastPos = platformBody.position;
    }

    private void FixedUpdate()
    {
        if (CompareTag("Horizontal Moving") && !isFalling)
        {
            HorizontalMoving();
        }

        if (CompareTag("Vertical Moving") && !isFalling)
        {
            VerticalMoving();
        }

        if (isOnPlatform && playerBody != null)
        {
            Vector2 platformDelta = platformBody.position - lastPos;
            playerBody.position += platformDelta;
        }

        lastPos = platformBody.position;
    }

    private void HorizontalMoving()
    {
        float targetX = movingRight ? startPos.x + tileMovingDistance : startPos.x - tileMovingDistance;
        Vector2 targetPos = new Vector2(targetX, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, tileMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            movingRight = !movingRight;
        }
    }

    private void VerticalMoving()
    {
        float targetY = movingUp ? startPos.y + tileMovingDistance : startPos.y - tileMovingDistance;
        Vector2 targetPos = new Vector2(transform.position.x, targetY);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, tileMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            movingUp = !movingUp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CompareTag("Falling"))
        {
            isFalling = true;
            Invoke(nameof(StartFalling), fallDelay);
        }

        else
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnPlatform = false;
        }
    }

    private void StartFalling()
    {
        platformBody.bodyType = RigidbodyType2D.Dynamic;
        platformBody.gravityScale = 2;
        SoundManager.Instance.PlayRockFallingSound();
        Destroy(gameObject, destroyTime);
    }
}