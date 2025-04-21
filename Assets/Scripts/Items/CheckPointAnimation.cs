using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointAnimation : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] protected GameObject pauseMenu;
    [SerializeField] private float colorChangeSpeed;
    [SerializeField] private TextMeshProUGUI requirementText;
    private Color targetColor;
    private ButtonManagerBase buttonManagerBase;
    private ParticleSystem endEffect;
    private Image panelImage;

    private void Awake()
    {
        endEffect = GetComponent<ParticleSystem>();
        buttonManagerBase = FindObjectOfType<ButtonManagerBase>();

        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (panelImage != null && endGamePanel.activeSelf)
        {
            panelImage.color = Color.Lerp(panelImage.color, targetColor, colorChangeSpeed * Time.unscaledDeltaTime);

            if (Vector4.Distance(panelImage.color, targetColor) < 0.02f) targetColor = GetRandomColor();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

        if (CoinManager.Instance.HasReachedTargetCoin())
        {
            endEffect.Play();
            CoinManager.Instance.HandleEndGame(playerMovement);
            Invoke(nameof(ShowEndGameMessage), 0.5f);
        }
        else
        {
            int coinsLeft = CoinManager.Instance.targetCoin - CoinManager.Instance.GetCoin();
            string coinWord = coinsLeft == 1 ? "coin" : "coins";

            requirementText.text = $"You need to collect {coinsLeft} more {coinWord} to win ^^";
            Invoke(nameof(HideRequirementText), 3f);
        }
    }

    private void ShowEndGameMessage()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            Time.timeScale = 0f;

            StartCoroutine(ReturnToGame(10f));
        }
    }

    private Color GetRandomColor()
    {
        return new Color(
            Random.Range(0.4f, 1f),
            Random.Range(0.4f, 1f),
            Random.Range(0.4f, 1f),
            1f
        );
    }

    private IEnumerator ReturnToGame(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f;
        endGamePanel.SetActive(false);
        pauseMenu.SetActive(true);
        buttonManagerBase.SetMouseOn();
    }

    private void HideRequirementText()
    {
        requirementText.text = "";
    }

}