using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float destroyDelay;
    private Animator _animator;
    private CircleCollider2D _collider;
    public string coinID;
    private bool isCollected = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CircleCollider2D>();

        GenerateCoinIDIfNeeded();

        if (SaveManager.IsCoinCollected(coinID))
        {
            Destroy(gameObject);
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

        Debug.Log($"[DEBUG] Coin {coinID} collected by player");

        if (_collider != null) _collider.enabled = false;

        SaveManager.MarkCoinCollected(coinID);

        HandleCoinPickup();
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
        CoinManager.Instance?.CountCoin(1);
    }

    private void PlayCoinAnimation()
    {
        if (CompareTag("Coin"))
        {
            _animator?.SetBool("isCollected", true);
        }
    }

    private void DisableCollider()
    {
        _collider.enabled = false;
    }

    private void ScheduleDestroy()
    {
        Invoke(nameof(DestroyCoin), destroyDelay);
    }

    private void DestroyCoin()
    {
        Destroy(gameObject);
    }
}