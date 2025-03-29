using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : BaseButtonManager
{
    protected override void OnButtonClicked(int index)
    {
        switch (index)
        {
            case 0:
                PlayEndSound();
                break;

            case 1:
                SceneManager.LoadScene("Gameplay Scene");
                Time.timeScale = 1f;
                break;

            case 2:
                PlayEndSound();
                OptionMenu();
                break;

            case 3:
                QuitGame();
                break;
        }
    }

    protected override void OptionMenu()
    {
        PlayEndSound();
        SetMouseOn();
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    private void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}