using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MainMenuManager : MonoBehaviour
{
    public GameObject playButton;

    public void Awake()
    {
        //BlankerAnimator.blanker.ResetBlanker();
    }

    private void Start()
    {
        _UIManager.UIManager.SetSelectedButton(playButton);
    }

    public void StartGame()
    {
        if (!BlankerAnimator.blanker.transitionInProgress)
        {
            StartCoroutine(FadeOut());
        }
    }

    public void QuitGame()
    {
        Debug.Log("You Can't Keep me here! !!");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public IEnumerator FadeOut()
    {
        HideCursor();
        BlankerAnimator.blanker.FadeOut();
        Debug.Log("Im gonna do a thing");
        while (BlankerAnimator.blanker.transitionInProgress)
        {
            yield return null;
        }
        Debug.Log("I did it");

        Loader.Load(Loader.Scene.SceneZero);
    }

    private void HideCursor()
    {
#if UNITY_EDITOR
        Cursor.visible = true;
#else
        Cursor.visible = false;
#endif
    }
}
