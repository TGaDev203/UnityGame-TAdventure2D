using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //! Components
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        InitializeComponents();
    }

    //! Initialization
    private void InitializeComponents()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    //! Moving Control
    private void Move()
    {
        rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         moveSpeed = -moveSpeed;
    //     }
    // }

    //! On Trigger Exit
    private void OnTriggerExit2D(Collider2D other)
    {
            moveSpeed = -moveSpeed;
    }
}
