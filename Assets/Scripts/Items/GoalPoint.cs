using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalPoint : MonoBehaviour
{
    [SerializeField] private float colorChangeSpeed;
    [SerializeField] private float deniedSoundCooldown;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private float lastDeniedSoundTime;
    [SerializeField] protected GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI requirementText;
    private ParticleSystem endEffect;
    private Image panelImage;
    private Color targetColor;

    private void HideRequirementText() => requirementText.text = string.Empty;

    private void Awake()
    {
        endEffect = GetComponent<ParticleSystem>();

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

            if (Time.time - lastDeniedSoundTime > deniedSoundCooldown)
            {
                lastDeniedSoundTime = Time.time;
                SoundManager.Instance.PlayGoalDeniedSound();
            }
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
    }
}