using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private int totalCoinCollected = 0;

    private void Awake()
    {
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
}
