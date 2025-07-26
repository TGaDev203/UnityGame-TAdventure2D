using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float destroyDelay;
    public string coinID;

    private Animator _animator;
    private CircleCollider2D _collider;
    private bool isCollected = false;

    private PlayerController playerController;

    private void DisableCollider() => _collider.enabled = false;
    private void DestroyCoin() => Destroy(gameObject);
    private void ScheduleDestroy() => Invoke(nameof(DestroyCoin), destroyDelay);

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CircleCollider2D>();

        GenerateCoinIDIfNeeded();

        if (SaveManager.IsCoinCollected(coinID))
        {
            gameObject.SetActive(false); // Ẩn nếu đã ăn coin này trước đó
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void GenerateCoinIDIfNeeded()
    {
        if (string.IsNullOrEmpty(coinID))
        {
            coinID = $"{transform.position.x}_{transform.position.y}";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected || !other.CompareTag("Player")) return;

        isCollected = true;

        _collider.enabled = false;

        SaveManager.MarkCoinCollected(coinID); // đánh dấu đã ăn coin này
        HandleCoinPickup();

        if (playerController != null)
        {
            // Lưu lại data người chơi sau khi ăn coin
            SaveManager.SavePlayerData(
                playerController.transform.position.x,
                playerController.transform.position.y,
                playerController.GetCurrentHealth(),
                CoinManager.Instance.GetCoin()
            );
        }
    }

    private void HandleCoinPickup()
    {
        IncrementCoinCount();
        PlayCoinAnimation();
        DisableCollider();
        SoundManager.Instance.PlayCoinSound();
        ScheduleDestroy();
    }

    private void IncrementCoinCount()
    {
        CoinManager.Instance?.CountCoin(1); // tăng coin count lên 1
    }

    private void PlayCoinAnimation()
    {
        if (_animator != null && CompareTag("Coin"))
        {
            _animator.SetBool("isCollected", true);
        }
    }
}
