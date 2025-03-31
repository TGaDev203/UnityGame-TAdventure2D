using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonManager : BaseButtonManager
{
    private void Start()
    {
        ToggleButton(0);
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

        if (optionMenu.activeSelf)
        {
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
            //!
            case 0:
                SoundManager.Instance.PlayMenuButtonEndSound();
                break;

            case 1:
                SoundManager.Instance.PlayMenuButtonEndSound();
                Invoke(nameof(LoadGameplayScene), 0.4f);
                Time.timeScale = 1f;
                break;

            case 2:
                SoundManager.Instance.PlayMenuButtonEndSound();
                OptionMenu();
                break;

            case 3:
                SoundManager.Instance.PlayMenuButtonEndSound();
                Invoke(nameof(QuitGame), 0.5f);
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
        UnityEditor.EditorApplication.isPlaying = false;
    }
}