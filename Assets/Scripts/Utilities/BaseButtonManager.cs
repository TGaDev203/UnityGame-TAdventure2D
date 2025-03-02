using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BaseButtonManager : MonoBehaviour
{
    [SerializeField] protected List<Button> buttons;
    protected int currentButtonIndex = -1;
    protected string sceneToLoad;
    protected bool canPlayEndSound = true;
    protected bool canPlayProgressSound = true;

    protected virtual void Awake()
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

    protected virtual void OnPointerEnter(int index)
    {
        PlayProgressSound();
        currentButtonIndex = index;
        UpdateButtons();
    }

    protected virtual void OnPointerExit(int index)
    {
        if (currentButtonIndex == index)
        {
            currentButtonIndex = -1;
            UpdateButtons();
        }
    }

    protected virtual void UpdateButtons()
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

    protected virtual void PlayEndSound()
    {
        if (canPlayEndSound)
        {
            SoundManager.Instance.PlayMenuButtonEndSound();
            canPlayEndSound = false;
            Invoke(nameof(ResetEndSoundCoolDown), 0.5f);
        }
    }

    protected virtual void PlayProgressSound()
    {
        if (canPlayProgressSound)
        {
            SoundManager.Instance.PlayMenuButtonProgressSound();
            canPlayProgressSound = false;
            Invoke(nameof(ResetProgressSoundCoolDown), 0.5f);
        }
    }

    private void ResetEndSoundCoolDown()
    {
        canPlayEndSound = true;
    }

    private void ResetProgressSoundCoolDown()
    {
        canPlayProgressSound = true;
    }
    protected virtual void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    protected abstract void OnButtonClicked(int index);
}
