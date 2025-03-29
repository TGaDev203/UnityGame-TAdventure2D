using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButtonManager : BaseButtonManager
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();

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

    private void Update()
    {
        if (player != null && player.IsDead()) return;
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (pauseMenu.activeSelf)
        {
            // SoundManager.Instance.PlayLoopSound();
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
        if (index == 0) OptionMenu();

        else if (index == 1) BackToMain();
    }

    private void Pause()
    {
        PlayProgressSound();
        SetMouseOn();
        pauseMenu.SetActive(true);
    }

    private void Resume()
    {
        PlayEndSound();
        SetMouseOff();
        pauseMenu.SetActive(false);
    }

    protected override void OptionMenu()
    {
        PlayEndSound();
        SetMouseOn();
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    private void BackToMain()
    {
        PlayEndSound();
        SceneManager.LoadScene("Main Menu Scene");
    }

    private void SetBGMVolume(float volume)
    {
        SoundManager.Instance.backgroundAudioSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    private void SetSFXVolume(float volume)
    {
        SoundManager.Instance.effectAudioSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}