using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    private TextMeshProUGUI coinText;
    private int totalCoinCollected = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        coinText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateCoinText();
    }

    public void CountCoin(int amount)
    {
        totalCoinCollected += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Total: " + totalCoinCollected.ToString();
        }
    }

    public int GetCoin()
    {
        return totalCoinCollected;
    }

    public void SetCoin(int value)
    {
        totalCoinCollected = value;
    }
}