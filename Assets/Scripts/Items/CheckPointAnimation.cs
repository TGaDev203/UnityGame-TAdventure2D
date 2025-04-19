using UnityEngine;
using UnityEngine.UI;

public class CheckPointAnimation : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] protected GameObject pauseMenu;
    private ButtonManagerBase buttonManagerBase;


    private ParticleSystem endEffect;
    private Image panelImage;
    private Color targetColor;
    private bool hasPlayed = false;
    private float colorChangeSpeed = 10f;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            endGamePanel.SetActive(false);

            pauseMenu.SetActive(true);
            buttonManagerBase.ToggleButton(0);
            buttonManagerBase.ToggleButton(1);
            buttonManagerBase.SetMouseOn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hasPlayed = true;
            endEffect.Play();

            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            panelImage = endGamePanel.GetComponentInChildren<Image>();
            if (panelImage != null)
            {
                targetColor = GetRandomColor();
            }

            SoundManager.Instance.PlayEndGameSound();
            Invoke(nameof(ShowEndGameMessage), 0.5f);
        }
    }

    private void ShowEndGameMessage()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            Time.timeScale = 0f;
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
}
