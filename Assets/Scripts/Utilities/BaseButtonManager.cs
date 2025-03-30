using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BaseButtonManager : MonoBehaviour
{
    protected bool isButtonClicked = false;
    [SerializeField] protected GameObject mainMenu;
    [SerializeField] protected GameObject pauseMenu;
    [SerializeField] protected GameObject optionMenu;
    [SerializeField] protected List<Button> buttons;
    protected int currentButtonIndex = -1;
    // protected bool canPlayEndSound = true;
    // protected bool canPlayProgressSound = true;
    protected bool showMenu = false;
    protected float volume = 1.0f;
    protected int resolutionIndex = 0;
        protected Player player;

    // protected enum MenuState { None, PauseMenu, OptionMenu }
    // protected MenuState currentMenuState = MenuState.None;

    protected void Awake()
    {
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

    protected void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu Scene");
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

    protected void BackToMain()
    {
        SoundManager.Instance.PlayMenuButtonEndSound();
        SceneManager.LoadScene("Main_Scene");
    }

    //     public void Option()
    // {
    //     PlayEndSound();
    //     SetMouseOn();
    //     pauseMenu.SetActive(false);
    //     optionMenu.SetActive(true);
    // }

    // protected void PlayEndSound()
    // {
    //     if (canPlayEndSound)
    //     {
    //         SoundManager.Instance.PlayMenuButtonEndSound();
    //         canPlayEndSound = false;
    //         Invoke(nameof(ResetEndSoundCoolDown), 0.5f);
    //     }
    // }

    // protected void PlayProgressSound()
    // {
    //     if (canPlayProgressSound)
    //     {
    //         SoundManager.Instance.PlayMenuButtonProgressSound();
    //         canPlayProgressSound = false;
    //         Invoke(nameof(ResetProgressSoundCoolDown), 0.5f);
    //     }
    // }

    // private void ResetEndSoundCoolDown()
    // {
    //     canPlayEndSound = true;
    // }

    // private void ResetProgressSoundCoolDown()
    // {
    //     canPlayProgressSound = true;
    // }

    protected void SetMouseOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    protected void SetMouseOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // public void ToggleButton(int index, bool isActive)
    // {
    //     if (index < 0 || index >= buttons.Count) return;
    //     buttons[index].gameObject.SetActive(isActive);
    // }
    protected abstract void HandlePauseInput();

    protected abstract void OnButtonClicked(int index);

    protected abstract void OptionMenu();
}
