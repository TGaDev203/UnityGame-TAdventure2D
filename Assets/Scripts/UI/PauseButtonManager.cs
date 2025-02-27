using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private float coolDownTime;
    private int currentButtonIndex;
    private string sceneToLoad;
    private float delayLoadScene;

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
                Back();
                break;
        }
    }

    private void OnPointerEnter (int index)
    {
        SoundManager.Instance.PlayMenuButtonProgressSound();
        currentButtonIndex = index;
        UpdateButton();
    }

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

    public void Back()
    {
        sceneToLoad = "Main Menu Scene";
        Invoke("LoadScene", delayLoadScene);
    }

    private void LoadScene ()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}