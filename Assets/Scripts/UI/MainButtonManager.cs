using UnityEngine;

public class MainButtonManager : ButtonManagerBase
{
    private void Start()
    {
        // ToggleButton(0);
        InitializeGameSettings();

        bool hasSave = SaveManager.SaveExists();
        SetButtonActive(0, hasSave);
    }

    private void Update()
    {
        HandlePauseInput();
    }

    protected override void HandlePauseInput()
    {
        if (player != null && player.IsDead()) return;
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (optionMenu.activeSelf)
        {
            SoundManager.Instance.PlayMenuButtonProgressSound();
            mainMenu.SetActive(true);
            optionMenu.SetActive(false);
        }
    }

    protected override void OnButtonClicked(int index)
    {
        if (isButtonClicked) return;

        isButtonClicked = true;
        Invoke(nameof(ResetButtonClick), 0.5f);

        switch (index)
        {
            case 0:
                SaveManager.LoadPlayerData();
                LoadGameplayScene();
                Time.timeScale = 1f;
                break;

            case 1:
                SaveManager.DeleteSave();
                LoadGameplayScene();
                Time.timeScale = 1f;
                break;

            case 2:
                SoundManager.Instance.PlayMenuButtonEndSound();
                OptionMenu();
                break;

            case 3:
                SoundManager.Instance.PlayMenuButtonEndSound();
                QuitGame();
                break;
        }
    }

    private void ResetButtonClick()
    {
        isButtonClicked = false;
    }

    protected override void OptionMenu()
    {
        SoundManager.Instance.PlayMenuButtonEndSound();
        SetMouseOn();
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}