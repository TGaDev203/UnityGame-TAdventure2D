using UnityEngine;
using UnityEngine.UI;

public class PauseButtonManager : ButtonManagerBase
{
    private void Start()
    {
        InitializeGameSettings();
    }

    private void Update()
    {
        HandlePauseInput();
    }

    protected override void HandlePauseInput()
    {
        if (player != null && player.IsDead()) return;
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (pauseMenu.activeSelf)
        {
            Resume();
            Time.timeScale = 1f;
        }

        else if (optionMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            optionMenu.SetActive(false);
        }

        else
        {
            Time.timeScale = 0f;
            Pause();
        }
    }

    protected override void OnButtonClicked(int index)
    {
        Button clickedButton = buttons[index];
        if (clickedButton.gameObject.name == "Replay") ReplayGame();

        else if (clickedButton.gameObject.name == "Option") OptionMenu();
        
        else LoadMainScene();
    }

    private void ReplayGame()
    {
        Invoke(nameof(LoadGameplayScene), 0.4f);
        Time.timeScale = 1f;
        Resume();
    }

    protected override void OptionMenu()
    {
        SoundManager.Instance.PlayMenuButtonEndSound();
        SetMouseOn();
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
}