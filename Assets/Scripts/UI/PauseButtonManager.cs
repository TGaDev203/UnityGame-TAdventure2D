using UnityEngine;

public class PauseButtonManager : BaseButtonManager
{
    [SerializeField] private GameObject pauseMenu;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (pauseMenu.activeSelf) Resume();

        else Pause();
    }

    protected override void OnButtonClicked(int index)
    {
        if (index == 1) Resume();

        else if (index == 3) Back();
    }

    private void Pause()
    {
        PlayProgressSound();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
    }

    private void Resume()
    {
        PlayEndSound();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.SetActive(false);
    }

    private void Back()
    {
        PlayEndSound();
        sceneToLoad = "Main Menu Scene";
        Invoke("LoadScene", 0.35f);
    }
}