using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BaseButtonManager : MonoBehaviour
{
    [SerializeField] protected GameObject mainMenu;
    [SerializeField] protected GameObject pauseMenu;
    [SerializeField] protected GameObject optionMenu;
    [SerializeField] protected List<Button> buttons;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    protected int currentButtonIndex = -1;
    protected bool isButtonClicked = false;
    protected bool showMenu = false;
    protected float volume = 1.0f;
    protected int resolutionIndex = 0;
    protected Player player;

    protected void LoadGameplayScene() => SceneManager.LoadScene("Gameplay_Scene");
    protected void LoadMainScene() => SceneManager.LoadScene("Main_Scene");
    protected abstract void HandlePauseInput();
    protected abstract void OnButtonClicked(int index);
    protected abstract void OptionMenu();

    protected void Awake()
    {
        player = FindObjectOfType<Player>();

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

        SoundManager.Instance.PlayMenuButtonProgressSound();
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

    protected void Pause()
    {
        SoundManager.Instance.PlayMenuButtonProgressSound();
        SetMouseOn();
        pauseMenu.SetActive(true);
    }

    protected void Resume()
    {
        SoundManager.Instance.PlayMenuButtonEndSound();
        SetMouseOff();
        pauseMenu.SetActive(false);
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
}
