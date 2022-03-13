using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public CameraBrain cameraBrain;

    public static bool GamePaused;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject debugMenuUI;

    public GameObject pauseFirstButton;
    public GameObject settingsFirstButton;
    public GameObject settingsClosedButton;

    private void Start()
    {
        Inputs._inputs.onOptionsCallback += ToggleOptions;
    }

    private void ToggleOptions()
    {
        if (GamePaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
#if UNITY_EDITOR
        debugMenuUI.SetActive(true);
#else 
        Cursor.visible = false;
#endif
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1f;
        Inputs.supressJump = false;
    }

    void Pause()
    {
#if UNITY_EDITOR
        debugMenuUI.SetActive(false);
#else 
        Cursor.visible = true;
#endif
        pauseMenuUI.SetActive(true);
        
        Time.timeScale = Time.timeScale == 0 ? 1: 0;
        GamePaused = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        Inputs.supressJump = true;
    }

    public void LoadMenu()
    {
        _GameManager.PrepForMenu();
        Loader.Load(Loader.Scene.MainMenu);
        cameraBrain.MainMenuPosition();
        _UIManager.UIManager.ResetMenus();
        _GameManager.firstUpdate = true;
        Resume();
    }

    public void SettingsMenu()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    public void SettingsExit()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsClosedButton);
    }

    public void PauseResume()
    {
        if (GamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void TransitionToMainMenu()
    {
        StartCoroutine(FadeOut());
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public IEnumerator FadeOut()
    {
        BlankerAnimator.blanker.FadeOut();

        while (BlankerAnimator.blanker.transitionInProgress)
        {
            yield return null;
        }

        LoadMenu();
    }
}
