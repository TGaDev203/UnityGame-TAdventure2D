using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private Rigidbody2D platformBody;

    // Moving Tile
    [SerializeField] private float tileMoveDistance;
    public float tileMoveSpeed;
    private Vector2 startPos;
    private bool movingRight = true;

    // Falling Tile
    [SerializeField] private float fallDelay;
    [SerializeField] private float destroyTime;
    private bool isFalling = false;

    // Player Control
    private bool isOnPlatform = false;
    private Rigidbody2D playerBody;
    private Vector2 lastPos;

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
        if (CompareTag("Moving") && !isFalling)
        {
            MovePlatform();
        }

        if (isOnPlatform && playerBody != null)
        {
            Vector2 platformDelta = platformBody.position - lastPos;
            playerBody.position += platformDelta;
        }

        lastPos = platformBody.position;
    }

    private void MovePlatform()
    {
        float targetX = movingRight ? startPos.x + tileMoveDistance : startPos.x - tileMoveDistance;
        Vector2 targetPos = new Vector2(targetX, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, tileMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            movingRight = !movingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
}