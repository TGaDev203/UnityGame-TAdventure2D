using UnityEngine;

public class PauseButtonManager : ButtonManagerBase
{
    private enum PauseButton
    {
        SaveAndMain = 0,
        Option = 1,
        Replay = 2
    }

    private void Start()
    {
        SetMouseOn();
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
        switch (index)
        {
            case (int)PauseButton.SaveAndMain:
                SaveAndReturnToMain();
                break;

            case (int)PauseButton.Option:
                OptionMenu();
                break;

            case (int)PauseButton.Replay:
                ReplayGame();
                break;

            default:
                Debug.LogWarning("Unknown button index: " + index);
                break;
        }
    }

    protected override void OptionMenu()
    {
        SoundManager.Instance.PlayMenuButtonEndSound();
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
}