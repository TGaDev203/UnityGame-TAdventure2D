using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;

public class PauseButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private float coolDownTime;
    private int currentButtonIndex;

    private void Awake()
    {
        AddButtonListeners();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                Resume();
            }

            else
            {
                SoundManager.Instance.StopAllSound();
                Pause();
            }
        }
    }

    // private void AddButtonListeners ()
    // {
    //     for (int i = 0; i < buttons.Count; i++)
    //     {
    //         int index = i;
            
    //         GameObject buttonGameObject = buttons[i].gameObject;

    //         EventTrigger trigger = buttonGameObject.AddComponent<EventTrigger>();

    //         EventTrigger.Entry entryEnter = new EventTrigger.Entry();
    //         entryEnter.eventID = EventTriggerType.PointerEnter;
    //         entryEnter.callback.AddListener((eventData) => { OnPointerEnter(index); });
    //         trigger.triggers.Add(entryEnter);

    //         EventTrigger.Entry entryExit = new EventTrigger.Entry();
    //         entryExit.eventID = EventTriggerType.PointerExit;
    //         entryExit.callback.AddListener((eventData) => { OnPointerExit(index); });
    //         trigger.triggers.Add(entryExit);

    //         buttons[i].onClick.AddListener(() => OnButtonClicked(index));
    //     }
    // }

    private void AddButtonListeners ()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;

            // Get the button component
            Button button = buttons[i];

            // Add event listeners using Unity's UI system
            EventTriggerListener.Get(button.gameObject).onEnter += (eventData) => OnPointerEnter(index);
            EventTriggerListener.Get(button.gameObject).onExit += (eventData) => OnPointerEnter(index);

            button.onClick.AddListener(() => OnButtonClicked(index)); 
        }
    }

    private void OnButtonClicked (int index)
    {
        switch (index)
        {
            case 0:
                Pause();
                break;

            case 1:
                Resume();
                break;

            case 2:
                break;

            case 3:
                Exit();
                break;
        }
    }

    private void OnPointerEnter (int index)
    {
        SoundManager.Instance.PlayMenuButtonProgressSound();
        currentButtonIndex = index;
        UpdateButton();
    }

    // private void OnPointerExit (int index)
    // {
    //     if (EventSystem.current.currentSelectedGameObject == buttons[currentButtonIndex].gameObject)
    //     {
    //         buttons[currentButtonIndex].OnDeselect(null);
    //     }
    // }

    private void UpdateButton ()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            UpdateButtonState(buttons[i], i == currentButtonIndex);
        }
    }

    private void UpdateButtonState (Button button, bool isSelected)
    {
        ColorBlock colors = button.colors;

        if (isSelected)
        {
            button.Select();
            colors.normalColor = Color.red;
        }

        else
        {
            button.OnDeselect(null);
            colors.normalColor = Color.white;
        }

        button.colors = colors;
    }

    public void Pause()
    {
        SoundManager.Instance.PlayMenuButtonProgressSound();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        SoundManager.Instance.PlayMenuButtonEndSound();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}