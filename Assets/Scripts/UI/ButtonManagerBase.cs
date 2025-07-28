using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class ButtonManagerBase : MonoBehaviour
{
    [SerializeField] protected List<Button> buttons;
    [SerializeField] protected GameObject mainMenu;
    [SerializeField] protected GameObject optionMenu;
    [SerializeField] protected GameObject pauseMenu;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    private const int MAINMENU_INDEX = 0;
    private const int GAMEPLAY_INDEX = 1;
    protected int currentButtonIndex = -1;
    protected bool isButtonClicked = false;
    protected PlayerController playerController;

    protected abstract void OnButtonClicked(int index);
    protected abstract void HandlePauseInput();
    protected abstract void OptionMenu();
    protected void LoadGameplayScene() => SceneManager.LoadScene(sceneBuildIndex: GAMEPLAY_INDEX);
    protected void LoadMainScene() => SceneManager.LoadScene(sceneBuildIndex: MAINMENU_INDEX);
    protected void QuitGame() => Application.Quit();
    protected void ResetButtonClick() => isButtonClicked = false;

    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            Button button = buttons[i];

            EventTriggerListener listener = EventTriggerListener.Get(button.gameObject);
            listener.onEnter += (eventData) => OnPointerEnter(index);
            listener.onExit += (eventData) => OnPointerExit(index);

            button.onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    protected void InitializeGameSettings()
    {
        // Load saved volume settings
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Apply loaded values
        SoundManager.Instance.backgroundAudioSource.volume = bgmSlider.value;
        SoundManager.Instance.effectAudioSource.volume = sfxSlider.value;

        // Add listeners to sliders
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    protected void OnPointerEnter(int index)
    {
        if (isButtonClicked) return;

        SoundManager.Instance.PlayButtonProgressSound();
        currentButtonIndex = index;
        UpdateButtons();
    }

    protected void OnPointerExit(int index)
    {
        if (currentButtonIndex == index)
        {
            currentButtonIndex = -1;
            UpdateButtons();
        }
    }

    protected void UpdateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            UpdateButtonState(buttons[i], i == currentButtonIndex);
        }
    }

    protected virtual void UpdateButtonState(Button button, bool isSelected)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = isSelected ? Color.red : Color.white;
        button.colors = colors;

        if (isSelected) button.Select();
        else button.OnDeselect(null);
    }

    public void SetMouseOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    protected void SetMouseOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void SetBGMVolume(float volume)
    {
        SoundManager.Instance.backgroundAudioSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    private void SetSFXVolume(float volume)
    {
        SoundManager.Instance.effectAudioSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void ToggleButton(int index)
    {
        if (index < 0 || index >= buttons.Count) return;

        Button button = buttons[index];
        button.gameObject.SetActive(!button.gameObject.activeSelf);
    }

    public void SetButtonActive(int index, bool isActive)
    {
        if (index < 0 || index >= buttons.Count) return;

        buttons[index].gameObject.SetActive(isActive);
    }

    protected void Pause()
    {
        SetMouseOn();
        SoundManager.Instance.PlayButtonProgressSound();
        pauseMenu.SetActive(true);
    }

    protected void Resume()
    {
        SetMouseOff();
        SoundManager.Instance.PlayButtonEndSound();
        pauseMenu.SetActive(false);
    }

    protected void SaveGame() => playerController?.SavePlayerData();

    protected void StartNewGame()
    {
        SaveManager.DeleteSave();
        LoadGameplayScene();
        Time.timeScale = 1f;
    }

    protected void ContinueGame()
    {
        SaveManager.LoadPlayerData();
        LoadGameplayScene();
        Time.timeScale = 1f;
    }

    protected void BackToMainMenu()
    {
        SoundManager.Instance.PlayButtonProgressSound();
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }
}