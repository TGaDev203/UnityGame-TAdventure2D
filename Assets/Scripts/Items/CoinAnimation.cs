using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    [SerializeField] private float destroyDelay;
    private Animator coinAnimation;
    private bool hasBeenPicked = false;

    void Awake()
    {
        coinAnimation = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenPicked && other.CompareTag("Player"))
        {
            HandleCoinPickup();
        }
    }

    private void HandleCoinPickup()
    {
        hasBeenPicked = true;
        IncrementCoinCount();
        CoinFlipAnimation();
        DisableCollider();
        SoundManager.Instance.PlayCoinSound();
        ScheduleDestroy();
    }

    private void IncrementCoinCount()
    {
        int coinValue = 1;
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.CountCoin(coinValue);
        }
    }

    private void CoinFlipAnimation()
    {
        if (gameObject.CompareTag("Coin"))
        {
            coinAnimation.SetBool("isCollected", true);
        }
    }

    private void DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false; 
    }

    private void ScheduleDestroy()
    {
        Invoke("DestroyCoin", destroyDelay);
    }

    private void DestroyCoin()
    {
        Destroy(gameObject);
    }
}