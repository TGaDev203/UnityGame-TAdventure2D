using UnityEngine;

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
                PlayEndSound();
                sceneToLoad = "Gameplay Scene";
                Invoke("LoadScene", 0.35f);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case 2:
                PlayEndSound();
                break;

            case 3:
                QuitGame();
                break;
        }
    }

    private void QuitGame()
    {
        // Application.Quit();
        // UnityEditor.EditorApplication.isPlaying = false;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();

#endif
    }
}