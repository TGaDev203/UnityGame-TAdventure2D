using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public TextMeshProUGUI totalCoinText;
    public TextMeshProUGUI targetCoinText;
    public int totalCoinCollected = 0;
    public int targetCoin;

    public int GetCoin() => totalCoinCollected;
    public bool HasReachedTargetCoin() => totalCoinCollected == targetCoin;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = 0;

        foreach (GameObject go in allGameObjects)
        {
            if (go.CompareTag("Coin") && go.hideFlags == HideFlags.None && go.scene.IsValid())
            {
                count++;
            }
        }

        targetCoin = count;
        UpdateTargetCoinText();
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
        if (totalCoinText != null)
        {
            totalCoinText.text = "Total: " + totalCoinCollected.ToString();
        }
    }

    private void UpdateTargetCoinText()
    {
        if (targetCoinText != null)
        {
            targetCoinText.text = "Target: " + targetCoin.ToString();
        }
    }

    public void SetCoin(int value)
    {
        totalCoinCollected = value;
        UpdateCoinText();
    }

    public void HandleEndGame(PlayerMovement playerMovement)
    {
        if (HasReachedTargetCoin())
        {
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            SoundManager.Instance.PlayEndGameSound();
        }
    }
}